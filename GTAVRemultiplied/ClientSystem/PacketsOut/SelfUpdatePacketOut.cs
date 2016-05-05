using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using FreneticScript;

namespace GTAVRemultiplied.ClientSystem.PacketsOut
{
    public class SelfUpdatePacketOut : AbstractPacketOut
    {
        public SelfUpdatePacketOut()
        {
            ID = ClientToServerPacket.SELF_UPDATE;
            Data = new byte[16 + 12 + 4];
            BitConverter.GetBytes(Game.Player.Character.Position.X).CopyTo(Data, 0);
            BitConverter.GetBytes(Game.Player.Character.Position.Y).CopyTo(Data, 4);
            BitConverter.GetBytes(Game.Player.Character.Position.Z).CopyTo(Data, 8);
            BitConverter.GetBytes(Game.Player.Character.Heading).CopyTo(Data, 12);
            Vector3 aim = Vector3.Zero;
            if (Game.Player.IsAiming)
            {
                aim = GameplayCamera.Direction;
            }
            BitConverter.GetBytes(aim.X).CopyTo(Data, 16);
            BitConverter.GetBytes(aim.Y).CopyTo(Data, 16 + 4);
            BitConverter.GetBytes(aim.Z).CopyTo(Data, 16 + 8);
            BitConverter.GetBytes((uint)Game.Player.Character.Weapons.Current.Hash).CopyTo(Data, 16 + 12);
        }
    }
}
