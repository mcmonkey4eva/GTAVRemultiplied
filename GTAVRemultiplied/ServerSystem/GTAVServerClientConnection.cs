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
using GTAVRemultiplied.ServerSystem.PacketsOut;

namespace GTAVRemultiplied.ServerSystem
{
    public class GTAVServerClientConnection
    {
        public Socket Sock;

        public bool InVehicle = false;

        public Model CharacterModel = PedHash.DeadHooker;

        public Ped Character;

        public HashSet<int> KnownCharHistory = new HashSet<int>();

        public Vector3 Aim = Vector3.Zero;

        public Vector3 lastShotAim = Vector3.Zero;

        public Blip blip;

        public Vector3 lPos = Vector3.Zero;

        public Vector3 lGoal = Vector3.Zero;

        public float speed = 0;

        public void AddBlip()
        {
            blip = Character.AttachBlip();
            blip.Sprite = BlipSprite.Standard;
            blip.Color = BlipColor.Blue;
        }

        public void SpawnCharacter()
        {
            Character = World.CreatePed(CharacterModel, Game.Player.Character.Position + Game.Player.Character.ForwardVector * 2);
            AddBlip();
            Character.IsPersistent = true;
            Character.IsInvincible = true;
            Character.IsFireProof = true;
            Character.IsExplosionProof = true;
            Weapon held = Character.Weapons.Give(WeaponHash.AdvancedRifle, 1000, true, true);
            Character.Weapons.Select(held);
            Character.CanBeDraggedOutOfVehicle = false;
            KnownCharHistory.Add(Character.Handle);
            //Character.FreezePosition = true;
        }

        public void Spawn()
        {
            SpawnCharacter();
            foreach (int vehicle in GTAVServerConnection.Vehicles.Keys)
            {
                SendPacket(new AddVehiclePacketOut(new Vehicle(vehicle)));
            }
            /*foreach (int prop in GTAVServerConnection.Props)
            {
                SendPacket(new AddPropPacketOut(new Prop(prop)));
            }*/
            foreach (int id in GTAVServerConnection.Characters.Keys)
            {
                Ped ped = new Ped(id);
                GTAVServerClientConnection owner = null;
                foreach (GTAVServerClientConnection connection in GTAVFreneticServer.Connections.Connections)
                {
                    if (connection.Character.Handle == ped.Handle)
                    {
                        owner = connection;
                        break;
                    }
                } 
                SendPacket(new AddPedPacketOut(ped));
                if (owner != null || Game.Player.Character.Handle == ped.Handle)
                {
                    SendPacket(new AddBlipPacketOut(ped, BlipSprite.Standard, BlipColor.Blue));
                }
            }
            SendPacket(new SetIPLDataPacketOut());
            SendPacket(new WorldStatusPacketOut());
            Log.Message("Server", "Spawned a new player from " + Sock.RemoteEndPoint.ToString());
        }

        byte[] known = new byte[8192 * 10];

        int count = 0;

        double lastRun = 0;

        bool running = false;

        public void AnimateMove(bool run)
        {
            if (Character.IsInVehicle())
            {
                return;
            }
            if (GTAVFreneticServer.GlobalTickTime - lastRun > 5)
            {
                if (run)
                {
                    Function.Call(Hash.REQUEST_ANIM_DICT, "MOVE_M@MULTIPLAYER");
                    Function.Call(Hash.TASK_PLAY_ANIM, Character.Handle, "MOVE_M@MULTIPLAYER", "run", 8f, 1f, 5000, 1, 1f, false, false, false);
                }
                else
                {
                    Function.Call(Hash.REQUEST_ANIM_DICT, "MOVE_M@NON_CHALANT");
                    Function.Call(Hash.TASK_PLAY_ANIM, Character.Handle, "MOVE_M@NON_CHALANT", "walk", 8f, 1f, 5000, 1, 1f, false, false, false);
                }
                lastRun = GTAVFreneticServer.GlobalTickTime;
                running = true;
            }
        }

        public void StopMove()
        {
            if (Character.IsInVehicle())
            {
                return;
            }
            lastRun = 0.0;
            if (running)
            {
                Function.Call(Hash.TASK_PLAY_ANIM, Character.Handle, "MOVE_M@MULTIPLAYER", "run", 8f, 1f, 0, 1, 1f, false, false, false);
                running = false;
            }
        }

        public void Tick()
        {
            if (dcon)
            {
                throw new Exception("Disconnected");
            }
            Vector3 rel = lGoal - lPos;
            float rlen = rel.Length();
            if (rlen > 0.01f && speed > 0.01f)
            {
                rel /= rlen;
                if (speed * GTAVFreneticServer.cDelta > rlen || rlen > 10)
                {
                    StopMove();
                    lPos = lGoal;
                    if (Character.IsInVehicle())
                    {
                        Character.CurrentVehicle.PositionNoOffset = lGoal;
                    }
                    else
                    {
                        Character.PositionNoOffset = lGoal;
                    }
                }
                else
                {
                    AnimateMove(speed > 3);
                    lPos = lPos + rel * speed * GTAVFreneticServer.cDelta;
                    if (Character.IsInVehicle())
                    {
                        Character.CurrentVehicle.PositionNoOffset = lPos;
                    }
                    else
                    {
                        Character.PositionNoOffset = lPos;
                    }
                }
            }
            else
            {
                StopMove();
                lPos = lGoal;
                if (Character.IsInVehicle())
                {
                    Character.CurrentVehicle.PositionNoOffset = lGoal;
                }
                else
                {
                    Character.PositionNoOffset = lGoal;
                }
            }
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
                            case ClientToServerPacket.SELF_UPDATE:
                                pack = new SelfUpdatePacketIn();
                                break;
                            case ClientToServerPacket.FIRED_SHOT:
                                pack = new FiredShotPacketIn();
                                break;
                            case ClientToServerPacket.JUMP:
                                pack = new JumpPacketIn();
                                break;
                            case ClientToServerPacket.ENTER_VEHICLE:
                                pack = new EnterVehiclePacketIn();
                                break;
                            case ClientToServerPacket.EXIT_VEHICLE:
                                pack = new ExitVehiclePacketIn();
                                break;
                            case ClientToServerPacket.REQUEST_MODEL:
                                pack = new RequestModelPacketIn();
                                break;
                        }
                        if (pack == null)
                        {
                            Log.Error("Packet from user is null!");
                            // TODO: Kick user + error.
                        }
                        else
                        {
                            if (!pack.ParseAndExecute(this, data))
                            {
                                Log.Error("Packet from user is invalid: " + packType);
                                // TODO: Kick user + error.
                            }
                        }
                    }
                }
            }
        }

        bool dcon = false;

        public void SendPacket(byte type, byte[] data)
        {
            if (dcon)
            {
                return;
            }
            try
            {
                byte[] toSend = new byte[data.Length + 5];
                toSend[0] = type;
                BitConverter.GetBytes(data.Length).CopyTo(toSend, 1);
                data.CopyTo(toSend, 5);
                Sock.Send(toSend);
            }
            catch (SocketException)
            {
                dcon = true;
            }
        }

        public void SendPacket(AbstractPacketOut pack)
        {
            SendPacket((byte)pack.ID, pack.Data);
        }
    }
}
