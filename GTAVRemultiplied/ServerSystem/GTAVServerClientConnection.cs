using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVRemultiplied.ServerSystem
{
    public class GTAVServerClientConnection
    {
        public Socket Sock;

        public Ped Character;

        public void Spawn()
        {
            Character = World.CreatePed(PedHash.DeadHooker, Game.Player.Character.Position + Game.Player.Character.ForwardVector * 2);
            Character.IsPersistent = true;
            Log.Message("Server", "Spawned a new player from " + Sock.RemoteEndPoint.ToString());
        }

        public void Tick()
        {
            // Placeholder!
            if (Sock.Available >= 12)
            {
                byte[] dat = new byte[12];
                Sock.Receive(dat, 12, SocketFlags.None);
                float x = BitConverter.ToSingle(dat, 0);
                float y = BitConverter.ToSingle(dat, 4);
                float z = BitConverter.ToSingle(dat, 8);
                Character.Position = new Vector3(x, y, z);
            }
        }

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
