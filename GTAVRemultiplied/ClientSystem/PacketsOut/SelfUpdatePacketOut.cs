using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using FreneticScript;
using GTAVRemultiplied.ServerSystem;

namespace GTAVRemultiplied.ClientSystem.PacketsOut
{
    public class SelfUpdatePacketOut : AbstractPacketOut
    {
        public SelfUpdatePacketOut()
        {
            ID = ClientToServerPacket.SELF_UPDATE;
            Data = new byte[16 + 12 + 4 + 12 + 12 + 1];
            Vector3 pos = (Game.Player.Character.CurrentVehicle != null ? Game.Player.Character.CurrentVehicle.Position : Game.Player.Character.Position);
            float head = (Game.Player.Character.CurrentVehicle != null ? Game.Player.Character.CurrentVehicle.Heading : Game.Player.Character.Heading);
            Vector3 vel = (Game.Player.Character.CurrentVehicle != null ? Game.Player.Character.CurrentVehicle.Velocity : Game.Player.Character.Velocity);
            BitConverter.GetBytes(pos.X).CopyTo(Data, 0);
            BitConverter.GetBytes(pos.Y).CopyTo(Data, 4);
            BitConverter.GetBytes(pos.Z).CopyTo(Data, 8);
            BitConverter.GetBytes(head).CopyTo(Data, 12);
            Vector3 aim = Vector3.Zero;
            if (Game.Player.IsAiming)
            {
                aim = GameplayCamera.Direction;
            }
            BitConverter.GetBytes(aim.X).CopyTo(Data, 16);
            BitConverter.GetBytes(aim.Y).CopyTo(Data, 16 + 4);
            BitConverter.GetBytes(aim.Z).CopyTo(Data, 16 + 8);
            BitConverter.GetBytes((uint)Game.Player.Character.Weapons.Current.Hash).CopyTo(Data, 16 + 12);
            BitConverter.GetBytes(vel.X).CopyTo(Data, 16 + 12 + 4);
            BitConverter.GetBytes(vel.Y).CopyTo(Data, 16 + 12 + 4 + 4);
            BitConverter.GetBytes(vel.Z).CopyTo(Data, 16 + 12 + 4 + 8);
            Vector3 rot = (Game.Player.Character.CurrentVehicle != null ? Game.Player.Character.CurrentVehicle.Rotation : Game.Player.Character.Rotation);
            BitConverter.GetBytes(rot.X).CopyTo(Data, 16 + 12 + 4 + 12);
            BitConverter.GetBytes(rot.Y).CopyTo(Data, 16 + 12 + 4 + 12 + 4);
            BitConverter.GetBytes(rot.Z).CopyTo(Data, 16 + 12 + 4 + 12 + 8);
            Data[16 + 12 + 4 + 12 + 12] = (byte)((Game.Player.Character.IsRunning ? PedFlags.RUNNING : 0)
                | (Game.Player.Character.IsSprinting ? PedFlags.SPRINTING : 0));
        }
    }
}
