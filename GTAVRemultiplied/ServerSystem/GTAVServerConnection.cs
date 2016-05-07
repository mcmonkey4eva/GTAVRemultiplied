using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using FreneticScript;
using GTA;
using GTA.Math;
using GTA.Native;
using GTAVRemultiplied.ServerSystem.PacketsOut;

namespace GTAVRemultiplied.ServerSystem
{
    public class GTAVServerConnection
    {
        public TcpListener Listener;

        public List<GTAVServerClientConnection> Connections = new List<GTAVServerClientConnection>();

        public void CheckForConnections()
        {
            while (Listener.Pending())
            {
                GTAVServerClientConnection client = new GTAVServerClientConnection();
                try
                {
                    client.Sock = Listener.AcceptSocket();
                    client.Sock.Blocking = false;
                    client.Sock.SendBufferSize = 1024 * 1024;
                    client.Sock.ReceiveBufferSize = 1024 * 1024;
                    Connections.Add(client);
                    client.Spawn();
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                }
            }
            for (int i = 0; i < Connections.Count; i++)
            {
                try
                {
                    Connections[i].Tick();
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                    Log.Error("Dropping a client!");
                    Connections[i].Sock.Close();
                    Connections.RemoveAt(i);
                    i--;
                }
            }
            HashSet<int> ids = new HashSet<int>(Vehicles);
            // TODO: Network vehicle updates more cleverly.
            bool needsVehUpdate = DateTime.Now.Subtract(nextVehicleUpdate).TotalMilliseconds > 100;
            if (needsVehUpdate)
            {
                nextVehicleUpdate = DateTime.Now;
            }
            foreach (Vehicle vehicle in World.GetAllVehicles())
            {
                if (Vehicles.Add(vehicle.Handle))
                {
                    foreach (GTAVServerClientConnection connection in Connections)
                    {
                        connection.SendPacket(new AddVehiclePacketOut(vehicle));
                    }
                }
                ids.Remove(vehicle.Handle);
                if (needsVehUpdate)
                {
                    foreach (GTAVServerClientConnection connection in Connections)
                    {
                        connection.SendPacket(new UpdateVehiclePacketOut(vehicle));
                    }
                }
            }
            foreach (int id in ids)
            {
                Vehicles.Remove(id);
                foreach (GTAVServerClientConnection connection in Connections)
                {
                    connection.SendPacket(new RemoveVehiclePacketOut(id));
                }
            }
            bool hasModel = ModelEnforcementScript.WantedModel.HasValue;
            if (hasModel)
            {
                int cModel = ModelEnforcementScript.WantedModel.Value.Hash;
                if (pModel != cModel)
                {
                    foreach (GTAVServerClientConnection connection in Connections)
                    {
                        connection.SendPacket(new SetModelPacketOut(Game.Player.Character, cModel));
                    }
                    pModel = cModel;
                }
            }
            pHadModel = hasModel;
            HashSet<int> pids = new HashSet<int>(Characters.Keys);
            foreach (Ped ped in World.GetAllPeds())
            {
                if (!Characters.ContainsKey(ped.Handle))
                {
                    Characters[ped.Handle] = new PedInfo();
                    foreach (GTAVServerClientConnection connection in Connections)
                    {
                        if (connection.Character.Handle != ped.Handle)
                        {
                            connection.SendPacket(new AddPedPacketOut(ped));
                        }
                    }
                }
                pids.Remove(ped.Handle);
                GTAVServerClientConnection owner = null;
                foreach (GTAVServerClientConnection connection in Connections)
                {
                    if (connection.Character.Handle == ped.Handle)
                    {
                        owner = connection;
                        break;
                    }
                }
                PedInfo character = Characters[ped.Handle];
                foreach (GTAVServerClientConnection connection in Connections)
                {
                    if (connection.Character.Handle != ped.Handle)
                    {
                        connection.SendPacket(new PlayerUpdatePacketOut(ped, owner == null ? Vector3.Zero: owner.Aim));
                        WeaponHash cweap = ped.Weapons.Current.Hash;
                        int cammo = ped.Weapons.Current.AmmoInClip;
                        if (cweap != character.weap)
                        {
                            character.weap = cweap;
                        }
                        else
                        {
                            if (character.ammo > cammo)
                            {
                                for (int i = 0; i < Connections.Count; i++)
                                {
                                    Connections[i].SendPacket(new FiredShotPacketOut(ped, owner == null ? Vector3.Zero : owner.Aim));
                                }
                            }
                            // TODO: Reload, etc.
                            // TODO: Sticky bombs, etc. No ammo clip!
                            // Also, sticky bomb remote detonation.
                        }
                        character.ammo = cammo;
                        bool tjump = Game.Player.Character.IsJumping;
                        if (tjump && !character.pjump)
                        {
                            for (int i = 0; i < Connections.Count; i++)
                            {
                                Connections[i].SendPacket(new JumpPacketOut(ped));
                            }
                        }
                        character.pjump = tjump;
                        bool isInVehicle = ped.IsSittingInVehicle();
                        if (isInVehicle && (!character.wasInVehicle || DateTime.Now.Subtract(character.nextVehicleReminder).TotalSeconds > 1.0))
                        {
                            character.nextVehicleReminder = DateTime.Now;
                            connection.SendPacket(new EnterVehiclePacketOut(ped, ped.CurrentVehicle, ped.SeatIndex));
                        }
                        else if (!isInVehicle && character.wasInVehicle)
                        {
                            connection.SendPacket(new ExitVehiclePacketOut(ped));
                        }
                        character.wasInVehicle = isInVehicle;
                    }
                }
            }
            foreach (int id in pids)
            {
                Characters.Remove(id);
                foreach (GTAVServerClientConnection connection in Connections)
                {
                    if (!connection.KnownCharHistory.Remove(id))
                    {
                        connection.SendPacket(new RemovePedPacketOut(id));
                    }
                }
            }
        }

        bool pHadModel;

        int pModel;
        
        DateTime nextVehicleUpdate = DateTime.Now;
        
        public static HashSet<int> Vehicles = new HashSet<int>();

        public static Dictionary<int, PedInfo> Characters = new Dictionary<int, PedInfo>();

        public void Listen(ushort port)
        {
            Listener = new TcpListener(IPAddress.IPv6Any, port);
            Listener.Server.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            Listener.Start();
        }
    }
}
