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

public class ClientConnectionScript : Script
{
    public ClientConnectionScript()
    {
        Tick += ClientConnectionScript_Tick;
    }

    Ped Character;

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
                }
                if (Connected)
                {
                    Character.Task.StandStill(100);
                    Vector3 pos = Game.Player.Character.Position;
                    byte[] dat = new byte[16];
                    BitConverter.GetBytes(pos.X).CopyTo(dat, 0);
                    BitConverter.GetBytes(pos.Y).CopyTo(dat, 4);
                    BitConverter.GetBytes(pos.Z).CopyTo(dat, 8);
                    BitConverter.GetBytes(Game.Player.Character.Heading).CopyTo(dat, 12);
                    Connection.Send(dat);
                    while (Connection.Available >= 16)
                    {
                        byte[] tdat = new byte[16];
                        Connection.Receive(tdat, 16, SocketFlags.None);
                        float x = BitConverter.ToSingle(tdat, 0);
                        float y = BitConverter.ToSingle(tdat, 4);
                        float z = BitConverter.ToSingle(tdat, 8);
                        float head = BitConverter.ToSingle(tdat, 12);
                        Character.PositionNoOffset = new Vector3(x, y, z);
                        Character.Heading = head;
                        if (!firsttele)
                        {
                            Game.Player.Character.PositionNoOffset = Character.Position;
                            firsttele = true;
                        }
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
