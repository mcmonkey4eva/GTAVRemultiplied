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
        public FiredShotPacketOut(Ped character, Vector3 aim)
        {
            // TODO: Player ID!
            ID = ServerToClientPacket.FIRED_SHOT;
            Data = new byte[12 + 4];
            BitConverter.GetBytes(aim.X).CopyTo(Data, 0);
            BitConverter.GetBytes(aim.Y).CopyTo(Data, 4);
            BitConverter.GetBytes(aim.Z).CopyTo(Data, 8);
            BitConverter.GetBytes(character.Handle).CopyTo(Data, 12);
        }
    }
}
