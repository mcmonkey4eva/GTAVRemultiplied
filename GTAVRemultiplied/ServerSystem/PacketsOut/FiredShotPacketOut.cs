using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    class FiredShotPacketOut : AbstractPacketOut
    {
        public FiredShotPacketOut(Player character) // TODO: GTAV-RMP Player object rather than GTA Player?
        {
            // TODO: Player ID!
            ID = ServerToClientPacket.FIRED_SHOT;
            Data = new byte[12];
            Vector3 aim = character.Character.ForwardVector;
            if (character.IsAiming)
            {
                aim = GameplayCamera.Direction;
            }
            BitConverter.GetBytes(aim.X).CopyTo(Data, 0);
            BitConverter.GetBytes(aim.Y).CopyTo(Data, 4);
            BitConverter.GetBytes(aim.Z).CopyTo(Data, 8);
        }
    }
}
