using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;
using System.Net;
using System.Net.Sockets;
using GTAVRemultiplied;
using GTAVRemultiplied.ClientSystem;
using GTAVRemultiplied.ClientSystem.PacketsOut;
using GTAVRemultiplied.ClientSystem.PacketsIn;

public class ClientConnectionScript : Script
{
    public ClientConnectionScript()
    {
        Tick += ClientConnectionScript_Tick;
    }

    public static Ped Character;

    byte[] known = new byte[8192 * 10];

    int count = 0;

    private void ClientConnectionScript_Tick(object sender, EventArgs e)
    {
        try
        {
            lock (Locker)
            {
                if (Connected && !pcon)
                {
                    pcon = true;
                    Log.Message("Connection", "Connected to a server now!");
                    Character = World.CreatePed(PedHash.DeadHooker, Game.Player.Character.Position + Game.Player.Character.ForwardVector * 2);
                    Character.IsPersistent = true;
                    Character.IsInvincible = true;
                    Character.IsFireProof = true;
                    Character.IsExplosionProof = true;
                    Weapon held = Character.Weapons.Give(WeaponHash.AdvancedRifle, 1000, true, true);
                    Character.Weapons.Select(held);
                }
                if (Connected)
                {
                    SendPacket(new SelfUpdatePacketOut());
                    while (Connection.Available > 0 && count < known.Length)
                    {
                        byte[] dat = new byte[known.Length - count];
                        int read = Connection.Receive(dat, Math.Min(dat.Length, Connection.Available), SocketFlags.None);
                        Array.Copy(dat, 0, known, count, read);
                        count += read;
                        while (count > 5)
                        {
                            ServerToClientPacket packType = (ServerToClientPacket)known[0];
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
                                    case ServerToClientPacket.PLAYER_UPDATE:
                                        pack = new PlayerUpdatePacketIn();
                                        break;
                                }
                                if (pack == null)
                                {
                                    Log.Message("Connection Error", "Packet from server is null!", 'Y');
                                    // TODO: Disconnect + error.
                                }
                                else
                                {
                                    if (!pack.ParseAndExecute(data))
                                    {
                                        Log.Message("Connection Error", "Packet from server is invalid!", 'Y');
                                        // TODO: Disconnect + error.
                                    }
                                }
                            }
                        }
                    }
                    if (!firsttele)
                    {
                        Game.Player.Character.PositionNoOffset = Character.Position;
                        firsttele = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
        }
    }

    bool pcon = false;

    public static bool firsttele = false;

    static Object Locker = new Object();

    public static bool Connected = false;

    static Socket Connection;

    public static void SendPacket(byte type, byte[] data)
    {
        byte[] toSend = new byte[data.Length + 5];
        toSend[0] = type;
        BitConverter.GetBytes(data.Length).CopyTo(toSend, 1);
        data.CopyTo(toSend, 5);
        Connection.Send(toSend);
    }

    public static void SendPacket(AbstractPacketOut pack)
    {
        SendPacket((byte)pack.ID, pack.Data);
    }

    public static void Connect(string ip, ushort port)
    {
        Connected = false;
        // TODO: Reasonably choose between ipv4 and ipv6.
        Connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Connection.BeginConnect(ip, port, (a) =>
        {
            Connection.EndConnect(a);
            if (Connection.Connected)
            {
                lock (Locker)
                {
                    Connected = true;
                }
            }
        }, null);
    }
}
