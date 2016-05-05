using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;

namespace GTAVRemultiplied.ClientSystem.PacketsOut
{
    class FiredShotPacketOut : AbstractPacketOut
    {
        public FiredShotPacketOut()
        {
            ID = ClientToServerPacket.FIRED_SHOT;
            Data = new byte[12];
            Vector3 aim = Game.Player.Character.ForwardVector;
            if (Game.Player.IsAiming)
            {
                aim = GameplayCamera.Direction;
            }
            BitConverter.GetBytes(aim.X).CopyTo(Data, 0);
            BitConverter.GetBytes(aim.Y).CopyTo(Data, 4);
            BitConverter.GetBytes(aim.Z).CopyTo(Data, 8);
        }
    }
}
