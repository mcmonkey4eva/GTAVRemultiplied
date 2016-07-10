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
using GTAVRemultiplied.SharedSystems;

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
            HashSet<int> ids = new HashSet<int>(Vehicles.Keys);
            foreach (Vehicle vehicle in World.GetAllVehicles())
            {
                if (!vehicle.Model.IsValid) // TODO: ???
                {
                    continue;
                }
                if (!Vehicles.ContainsKey(vehicle.Handle))
                {
                    VehicleInfo vi = new VehicleInfo();
                    Vehicles.Add(vehicle.Handle, vi);
                    foreach (GTAVServerClientConnection connection in Connections)
                    {
                        connection.SendPacket(new AddVehiclePacketOut(vehicle));
                    }
                }
                ids.Remove(vehicle.Handle);
                VehicleInfo vinf = Vehicles[vehicle.Handle];
                bool inRange = GTAVFreneticServer.IsInRangeOfPlayer(vehicle.Position);
                if (inRange && !vehicle.IsPersistent && !vinf.ForcePersistent)
                {
                    vinf.ForcePersistent = true;
                    vehicle.IsPersistent = true;
                }
                if (!inRange && vinf.ForcePersistent)
                {
                    vinf.ForcePersistent = false;
                    vehicle.IsPersistent = false;
                }
                foreach (GTAVServerClientConnection connection in Connections)
                {
                    connection.SendPacket(new UpdateVehiclePacketOut(vehicle));
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
            HashSet<int> propids = new HashSet<int>(Props);
            deltaTilPropUpdate -= GTAVFreneticServer.cDelta;
            /*foreach (Prop prop in World.GetAllProps())
            {
                if (!prop.Model.IsValid || !Enum.IsDefined(typeof(PropHash), prop.Model.Hash)) // TODO: ???
                {
                    continue;
                }
                if (Props.Add(prop.Handle))
                {
                    foreach (GTAVServerClientConnection connection in Connections)
                    {
                        connection.SendPacket(new AddPropPacketOut(prop));
                    }
                }
                propids.Remove(prop.Handle);
                if (deltaTilPropUpdate < 0)
                {
                    // TODO: Don't update if it hasn't moved!
                    foreach (GTAVServerClientConnection connection in Connections)
                    {
                        connection.SendPacket(new UpdatePropPacketOut(prop));
                    }
                }
            }
            if (deltaTilPropUpdate < 0)
            {
                deltaTilPropUpdate = 0.1f;
            }
            foreach (int id in propids)
            {
                Props.Remove(id);
                foreach (GTAVServerClientConnection connection in Connections)
                {
                    connection.SendPacket(new RemovePropPacketOut(id));
                }
            }*/
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
                if (!ped.Model.IsValid) // TODO: ???
                {
                    continue;
                }
                GTAVServerClientConnection owner = null;
                foreach (GTAVServerClientConnection connection in Connections)
                {
                    if (connection.Character.Handle == ped.Handle)
                    {
                        owner = connection;
                        break;
                    }
                }
                if (!Characters.ContainsKey(ped.Handle))
                {
                    Characters[ped.Handle] = new PedInfo();
                    foreach (GTAVServerClientConnection connection in Connections)
                    {
                        if (connection.Character.Handle != ped.Handle)
                        {
                            connection.SendPacket(new AddPedPacketOut(ped));
                            if (owner != null || Game.Player.Character.Handle == ped.Handle)
                            {
                                connection.SendPacket(new AddBlipPacketOut(ped, BlipSprite.Standard, BlipColor.Blue));
                            }
                        }
                    }
                }
                pids.Remove(ped.Handle);
                PedInfo character = Characters[ped.Handle];
                bool inRange = GTAVFreneticServer.IsInRangeOfPlayer(ped.Position);
                if (inRange && !ped.IsPersistent && !character.ForcePersistent)
                {
                    character.ForcePersistent = true;
                    ped.IsPersistent = true;
                }
                if (!inRange && character.ForcePersistent)
                {
                    character.ForcePersistent = false;
                    ped.IsPersistent = false;
                }
                WeaponHash cweap = ped.Weapons.Current.Hash;
                int cammo = ped.Weapons.Current.AmmoInClip;
                bool tjump = Game.Player.Character.IsJumping;
                bool isInVehicle = ped.IsSittingInVehicle();
                bool vehRem = isInVehicle && (!character.wasInVehicle || DateTime.Now.Subtract(character.nextVehicleReminder).TotalSeconds > 1.0);
                foreach (GTAVServerClientConnection connection in Connections)
                {
                    if (connection.Character.Handle != ped.Handle)
                    {
                        connection.SendPacket(new PlayerUpdatePacketOut(ped,
                            (ped.Handle == Game.Player.Character.Handle && Game.Player.IsAiming) ? GameplayCamera.Direction : ((owner == null) ? Vector3.Zero : owner.Aim)));
                        if (cweap == character.weap && character.ammo > cammo)
                        {
                            connection.SendPacket(new FiredShotPacketOut(ped,
                                (ped.Handle == Game.Player.Character.Handle) ? GameplayCamera.Direction : ((owner == null) ? ped.ForwardVector : owner.lastShotAim)));
                            // TODO: Reload, etc.
                            // TODO: Sticky bombs, etc. No ammo clip!
                            // Also, sticky bomb remote detonation.
                        }
                        if (tjump && !character.pjump)
                        {
                            connection.SendPacket(new JumpPacketOut(ped));
                        }
                        if (vehRem)
                        {
                            connection.SendPacket(new EnterVehiclePacketOut(ped, ped.CurrentVehicle, ped.SeatIndex));
                        }
                        else if (!isInVehicle && character.wasInVehicle)
                        {
                            connection.SendPacket(new ExitVehiclePacketOut(ped));
                        }
                    }
                }
                character.weap = cweap;
                character.ammo = cammo;
                character.pjump = tjump;
                character.wasInVehicle = isInVehicle;
                if (vehRem)
                {
                    character.nextVehicleReminder = DateTime.Now;
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
            deltaTilWorldUpdate -= GTAVFreneticServer.cDelta;
            if (deltaTilWorldUpdate < 0)
            {
                deltaTilWorldUpdate = 0.5f;
                foreach (GTAVServerClientConnection connection in Connections)
                {
                    connection.SendPacket(new WorldStatusPacketOut());
                }
            }
        }

        bool pHadModel;

        int pModel;

        float deltaTilPropUpdate = 0;
        float deltaTilWorldUpdate = 0;
        
        public static Dictionary<int, VehicleInfo> Vehicles = new Dictionary<int, VehicleInfo>();

        public static HashSet<int> Props = new HashSet<int>();

        public static Dictionary<int, PedInfo> Characters = new Dictionary<int, PedInfo>();

        public void Listen(ushort port)
        {
            Listener = new TcpListener(IPAddress.IPv6Any, port);
            Listener.Server.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            Listener.Start();
        }
    }
}
