using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace GTAVRemultiplied.ServerSystem
{
    public class GTAVServerClientConnection
    {
        public Socket Sock;

        public void SendPacket(byte type, byte[] data)
        {
            byte[] toSend = new byte[data.Length + 5];
            toSend[0] = type;
            BitConverter.GetBytes(data.Length).CopyTo(toSend, 1);
            data.CopyTo(toSend, 5);
            Sock.Send(toSend);
        }

        public void SendPacket(AbstractPacketOut pack)
        {
            SendPacket(pack.ID, pack.Data);
        }
    }
}
