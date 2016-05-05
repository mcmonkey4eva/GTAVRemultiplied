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
using GTAVRemultiplied.ServerSystem.PacketsIn;

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
            Character.IsInvincible = true;
            Character.IsFireProof = true;
            Character.IsExplosionProof = true;
            Weapon held = Character.Weapons.Give(WeaponHash.AdvancedRifle, 1000, true, true);
            Character.Weapons.Select(held);
            Log.Message("Server", "Spawned a new player from " + Sock.RemoteEndPoint.ToString());
        }

        byte[] known = new byte[8192 * 10];

        int count = 0;

        public void Tick()
        {
            while (Sock.Available > 0 && count < known.Length)
            {
                byte[] dat = new byte[known.Length - count];
                int read = Sock.Receive(dat, Math.Min(dat.Length, Sock.Available), SocketFlags.None);
                Array.Copy(dat, 0, known, count, read);
                count += read;
                while (count > 5)
                {
                    ClientToServerPacket packType = (ClientToServerPacket)known[0];
                    int len = BitConverter.ToInt32(known, 1);
                    if (count >= len + 5)
                    {
                        byte[] data = new byte[len];
                        Array.Copy(known, 5, data, 0, len);
                        count -= len + 5;
                        Array.Copy(known, len + 5, known, 0, count);
                        AbstractPacketIn pack = null;
                        switch (packType)
                        {
                            case ClientToServerPacket.SELF_UPDATE:
                                pack = new SelfUpdatePacketIn();
                                break;
                            case ClientToServerPacket.FIRED_SHOT:
                                pack = new FiredShotPacketIn();
                                break;
                        }
                        if (pack == null)
                        {
                            Log.Message("Server Error", "Packet from user is null!", 'Y');
                            // TODO: Kick user + error.
                        }
                        else
                        {
                            if (!pack.ParseAndExecute(this, data))
                            {
                                Log.Message("Server Error", "Packet from user is invalid!", 'Y');
                                // TODO: Kick user + error.
                            }
                        }
                    }
                }
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
            SendPacket((byte)pack.ID, pack.Data);
        }
    }
}
