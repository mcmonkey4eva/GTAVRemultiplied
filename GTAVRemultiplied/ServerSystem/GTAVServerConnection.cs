using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using FreneticScript;

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
                    byte[] dat = FreneticScriptUtilities.Enc.GetBytes("Server is at " + GTA.Game.Player.Character.Position + "\n");
                    Connections[i].Sock.Send(dat);
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
            /*
            Listener = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            Listener.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            Listener.Bind(new IPEndPoint(IPAddress.IPv6Any, port));
            Listener.Listen(10);*/
            Listener = new TcpListener(IPAddress.IPv6Any, port);
            Listener.Server.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            Listener.Start();
        }
    }
}
