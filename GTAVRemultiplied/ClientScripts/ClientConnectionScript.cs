using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using GTAVRemultiplied;
using GTAVRemultiplied.ClientSystem;
using GTAVRemultiplied.ClientSystem.PacketsOut;
using GTAVRemultiplied.ClientSystem.PacketsIn;
using GTAVRemultiplied.SharedSystems;
using GTAVRemultiplied.ClientSystem.TagBases;

public class ClientConnectionScript : Script
{
    public ClientConnectionScript()
    {
        Tick += ClientConnectionScript_Tick;
    }

    public static bool DebugProps = false;
    
    byte[] known = new byte[8192 * 10];

    int count = 0;

    int ammo = 0;
    WeaponHash weap = WeaponHash.Unarmed;

    bool pjump = false;

    bool wasInVehicle = false;

    public static Dictionary<int, int> ServerToClientVehicle = new Dictionary<int, int>();

    public static Dictionary<int, int> ClientToServerVehicle = new Dictionary<int, int>();

    public static Dictionary<int, int> ServerToClientPed = new Dictionary<int, int>();

    public static Dictionary<int, int> ClientToServerPed = new Dictionary<int, int>();

    public static Dictionary<int, PedInfo> ServerPedKnownPosition = new Dictionary<int, PedInfo>();

    public static Dictionary<int, VehicleInfo> ServerVehKnownPosition = new Dictionary<int, VehicleInfo>();

    public static Dictionary<int, int> ServerToClientProp = new Dictionary<int, int>();

    public static Dictionary<int, int> ClientToServerProp = new Dictionary<int, int>();

    bool dlcEnabled = false;

    public static void Text3D(Vector3 pos, string text)
    {
        Text3D(pos, text, System.Drawing.Color.White, 5f);
    }

    public static void Text3D(Vector3 pos, string text, System.Drawing.Color color, float sfact)
    {
        if (camPos.DistanceToSquared(pos) < 400)
        {
            System.Drawing.PointF point = GTA.UI.Screen.WorldToScreen(pos);
            if (point.Y <= 0 || point.Y >= Screen.Height || point.X <= 0 || point.X >= Screen.Width)
            {
                return;
            }
            float dist = camPos.DistanceTo(pos);
            float scale = sfact / (dist / GameplayCamera.Zoom);
            TextElement utext = new TextElement(text, point, scale, color, Font.ChaletLondon, Alignment.Center, true, false);
            utext.Draw();
        }
    }

    public static Vector3 camPos;

    public double VehicleAnnounceTimer = 0.0;

    private void ClientConnectionScript_Tick(object sender, EventArgs e)
    {
        try
        {
            camPos = GameplayCamera.Position;
            if (DebugProps)
            {
                foreach (Prop prop in World.GetAllProps())
                {
                    Vector3 pos = prop.Position;
                    string modname = ((PropHash)prop.Model.Hash).ToString();
                    Text3D(prop.Position, "<" + modname + ">");
                }
            }
            if (!dlcEnabled)
            {
                GTAVUtilities.EnableDLC();
                dlcEnabled = true;
            }
            lock (Locker)
            {
                if (Connected && !pcon)
                {
                    pcon = true;
                    Function.Call(Hash.SET_RANDOM_TRAINS, false);
                    Log.Message("Connection", "Connected to a server now!");
                }
                if (Connected)
                {
                    if (DebugPositionScript.Enabled)
                    {
                        foreach (KeyValuePair<int, PedInfo> peddata in ServerPedKnownPosition)
                        {
                            Ped p = new Ped(ServerToClientPed[peddata.Key]);
                            PedHash model = (PedHash)unchecked((uint)p.Model.Hash);
                            string modname = model.ToString();
                            Vector3 pos = peddata.Value.WorldPos;
                            System.Drawing.PointF point = Screen.WorldToScreen(pos);
                            Vector3 camPos = GameplayCamera.Position;
                            float dist = camPos.DistanceTo(pos);
                            float scale = 5f / (dist / GameplayCamera.Zoom);
                            TextElement text = new TextElement("<" + modname + ">", point, scale);
                            text.Draw();
                        }
                    }
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
                            if (count < len + 5)
                            {
                                break;
                            }
                            else
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
                                    case ServerToClientPacket.FIRED_SHOT:
                                        pack = new FiredShotPacketIn();
                                        break;
                                    case ServerToClientPacket.JUMP:
                                        pack = new JumpPacketIn();
                                        break;
                                    case ServerToClientPacket.ENTER_VEHICLE:
                                        pack = new EnterVehiclePacketIn();
                                        break;
                                    case ServerToClientPacket.EXIT_VEHICLE:
                                        pack = new ExitVehiclePacketIn();
                                        break;
                                    case ServerToClientPacket.SET_MODEL:
                                        pack = new SetModelPacketIn();
                                        break;
                                    case ServerToClientPacket.ADD_VEHICLE:
                                        pack = new AddVehiclePacketIn();
                                        break;
                                    case ServerToClientPacket.REMOVE_VEHICLE:
                                        pack = new RemoveVehiclePacketIn();
                                        break;
                                    case ServerToClientPacket.UPDATE_VEHICLE:
                                        pack = new UpdateVehiclePacketIn();
                                        break;
                                    case ServerToClientPacket.ADD_PED:
                                        pack = new AddPedPacketIn();
                                        break;
                                    case ServerToClientPacket.REMOVE_PED:
                                        pack = new RemovePedPacketIn();
                                        break;
                                    case ServerToClientPacket.SET_IPL_DATA:
                                        pack = new SetIPLDataPacketIn();
                                        break;
                                    case ServerToClientPacket.WORLD_STATUS:
                                        pack = new WorldStatusPacketIn();
                                        break;
                                    case ServerToClientPacket.ADD_BLIP:
                                        pack = new AddBlipPacketIn();
                                        break;
                                    case ServerToClientPacket.ADD_PROP:
                                        pack = new AddPropPacketIn();
                                        break;
                                    case ServerToClientPacket.REMOVE_PROP:
                                        pack = new RemovePropPacketIn();
                                        break;
                                    case ServerToClientPacket.UPDATE_PROP:
                                        pack = new UpdatePropPacketIn();
                                        break;
                                    case ServerToClientPacket.PING:
                                        pack = new PingPacketIn();
                                        break;
                                }
                                if (pack == null)
                                {
                                    Log.Error("Packet from server is null: " + packType);
                                    Connected = false;
                                    // TODO: Disconnect properly.
                                    return;
                                }
                                else
                                {
                                    if (!pack.ParseAndExecute(data))
                                    {
                                        Log.Error("Packet from server is invalid: " + packType + " of length " + data.Length);
                                        Connected = false;
                                        // TODO: Disconnect properly.
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    // TODO: let the server decide a spawn point!
                    if (!firsttele && ClientToServerPed.Count > 0)
                    {
                        Vector3 pos = new Ped(ClientToServerPed.Keys.First()).Position;
                        foreach (int ped in ClientToServerPed.Keys)
                        {
                            if (new Ped(ped).Model.Hash == DefaultModel.Hash)
                            {
                                pos = new Ped(ped).Position;
                                break;
                            }
                        }
                        Game.Player.Character.PositionNoOffset = pos;
                        firsttele = true;
                    }
                    WeaponHash cweap = Game.Player.Character.Weapons.Current.Hash;
                    int cammo = Game.Player.Character.Weapons.Current.AmmoInClip;
                    if (cweap != weap)
                    {
                        weap = cweap;
                    }
                    else
                    {
                        if (ammo > cammo)
                        {
                            SendPacket(new FiredShotPacketOut());
                        }
                        // TODO: Reload, etc.
                    }
                    ammo = cammo;
                    bool tjump = Game.Player.Character.IsJumping;
                    if (tjump && !pjump)
                    {
                        SendPacket(new JumpPacketOut());
                    }
                    pjump = tjump;
                    /*foreach (Prop prop in World.GetAllProps())
                    {
                        if (!prop.Model.IsValid || !Enum.IsDefined(typeof(PropHash), prop.Model.Hash))
                        {
                            continue;
                        }
                        if (!ClientToServerProp.ContainsKey(prop.Handle))
                        {
                            prop.Delete();
                        }
                    }*/
                    foreach (Vehicle vehicle in World.GetAllVehicles())
                    {
                        if (!ClientToServerVehicle.ContainsKey(vehicle.Handle))
                        {
                            vehicle.Delete();
                        }
                    }
                    foreach (Ped ped in World.GetAllPeds())
                    {
                        if (!ClientToServerPed.ContainsKey(ped.Handle) && ped.Handle != Game.Player.Character.Handle)
                        {
                            ped.Delete();
                        }
                    }
                    bool isInVehicle = Game.Player.Character.CurrentVehicle != null;
                    VehicleAnnounceTimer += GTAVFrenetic.cDelta;
                    bool anncVeh = VehicleAnnounceTimer > 1.0; // TODO: Constant
                    if (isInVehicle && (anncVeh || !wasInVehicle))
                    {
                        SendPacket(new EnterVehiclePacketOut(Game.Player.Character.CurrentVehicle, Game.Player.Character.SeatIndex));
                        VehicleAnnounceTimer = 0.0;
                    }
                    else if (!isInVehicle && (anncVeh || wasInVehicle))
                    {
                        SendPacket(new ExitVehiclePacketOut());
                        VehicleAnnounceTimer = 0.0;
                    }
                    wasInVehicle = isInVehicle;
                    bool hasModel = ModelEnforcementScript.WantedModel.HasValue;
                    if (hasModel)
                    {
                        int cModel = ModelEnforcementScript.WantedModel.Value.Hash;
                        if (pModel != cModel)
                        {
                            SendPacket(new RequestModelPacketOut(cModel));
                            pModel = cModel;
                        }
                    }
                    pHadModel = hasModel;
                    foreach (PedInfo pinf in ServerPedKnownPosition.Values)
                    {
                        pinf.Tick();
                        if (pinf.Name != null)
                        {
                            Text3D(pinf.Character.GetBoneCoord(Bone.IK_Head) + new Vector3(0, 0, 0.25f), pinf.Name, System.Drawing.Color.White, 2f);
                        }
                    }
                    foreach (VehicleInfo inf in ServerVehKnownPosition.Values)
                    {
                        inf.Tick();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
            if (ex is SocketException)
            {
                Connected = false;
                // TODO: Disconnect properly!
            }
            // TODO: /maybe/ disconnect anyway?
        }
    }

    static Model DefaultModel = PedHash.DeadHooker;

    bool pcon = false;

    bool pHadModel;

    int pModel;

    public static bool firsttele = false;

    static Object Locker = new Object();

    public static bool Connected = false;

    static Socket Connection;

    public static void SendPacket(byte type, byte[] data)
    {
        // TODO: Data waste monitoring / slowdown as needed?
        byte[] toSend = new byte[data.Length + 5];
        toSend[0] = type;
        BitConverter.GetBytes(data.Length).CopyTo(toSend, 1);
        data.CopyTo(toSend, 5);
        GTAVRemultiplied.ServerSystem.GTAVFreneticServer.DataUsage += toSend.Length;
        Connection.Send(toSend);
    }

    public static void SendPacket(AbstractPacketOut pack)
    {
        SendPacket((byte)pack.ID, pack.Data);
    }
    
    public static void Connect(string ip, ushort port)
    {
        try
        {
            if (!ModelEnforcementScript.WantedModel.HasValue)
            {
                GTAVUtilities.SwitchCharacter(PedHash.DeadHooker);
            }
            Connected = false;
            // TODO: Reasonably choose between ipv4 and ipv6.
            Connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { SendBufferSize = 1024 * 1024 * 10, ReceiveBufferSize = 1024 * 1024 * 10 };
            Connection.BeginConnect(ip, port, (a) =>
            {
                try
                {
                    Connection.EndConnect(a);
                    if (Connection.Connected)
                    {
                        string sess = AccountHelper.GetWebSession();
                        Connection.Send(GTAVUtilities.Enc.GetBytes(AccountHelper.Username + "\n" + sess + "\n\n"));
                        int count = 0;
                        string b = "";
                        while (count < 100)
                        {
                            while (Connection.Available > 0)
                            {
                                byte[] hb = new byte[1];
                                int c = Connection.Receive(hb, 1, SocketFlags.None);
                                b += GTAVUtilities.Enc.GetString(hb, 0, c);
                                if (b.EndsWith("\n\n"))
                                {
                                    if (b.StartsWith("ACCEPT"))
                                    {
                                        lock (Locker)
                                        {
                                            Connected = true;
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Connection rejected by server!");
                                    }
                                }
                            }
                            Thread.Sleep(100);
                            count++;
                        }
                        throw new Exception("Failed to connect: timeout!");
                    }
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                    Connection.Close();
                    Connected = false;
                }
            }, null);
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
        }
    }
}
