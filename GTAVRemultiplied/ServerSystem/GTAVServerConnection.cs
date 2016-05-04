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
                catch (Exception)
                {
                    // Do nothing!
                }
            }
            for (int i = 0; i < Connections.Count; i++)
            {
                try
                {
                    // Placeholder!
                    Vector3 pos = Game.Player.Character.Position;
                    byte[] dat = new byte[16];
                    BitConverter.GetBytes(pos.X).CopyTo(dat, 0);
                    BitConverter.GetBytes(pos.Y).CopyTo(dat, 4);
                    BitConverter.GetBytes(pos.Z).CopyTo(dat, 8);
                    BitConverter.GetBytes(Game.Player.Character.Heading).CopyTo(dat, 12);
                    Connections[i].Sock.Send(dat);
                    Connections[i].Tick();
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
        }

        public void Listen(ushort port)
        {
            Listener = new TcpListener(IPAddress.IPv6Any, port);
            Listener.Server.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            Listener.Start();
        }
    }
}
