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
                    client.Sock.SendBufferSize = 1024 * 10 * 8;
                    client.Sock.ReceiveBufferSize = 1024 * 10 * 8;
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
                    Connections[i].SendPacket(new PlayerUpdatePacketOut(Game.Player));
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                    Log.Message("Server", "Dropping a client!", 'Y');
                    Connections[i].Sock.Close();
                    Connections.RemoveAt(i);
                    i--;
                }
            }
            WeaponHash cweap = Game.Player.Character.Weapons.Current.Hash;
            if (cweap != weap)
            {
                weap = cweap;
            }
            else
            {
                int cammo = Game.Player.Character.Weapons.Current.AmmoInClip;
                if (ammo > cammo)
                {
                    for (int i = 0; i < Connections.Count; i++)
                    {
                        Connections[i].SendPacket(new FiredShotPacketOut(Game.Player));
                    }
                }
                ammo = cammo;
                // TODO: Reload, etc.
            }
        }
        
        int ammo = 0;
        WeaponHash weap = WeaponHash.Unarmed;

        public void Listen(ushort port)
        {
            Listener = new TcpListener(IPAddress.IPv6Any, port);
            Listener.Server.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            Listener.Start();
        }
    }
}
