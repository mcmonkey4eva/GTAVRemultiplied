using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class SetModelPacketOut : AbstractPacketOut
    {
        public SetModelPacketOut(Ped character, int modhash)
        {
            ID = ServerToClientPacket.SET_MODEL;
            Data = new byte[4 + 4];
            BitConverter.GetBytes(modhash).CopyTo(Data, 0);
            BitConverter.GetBytes(character.Handle).CopyTo(Data, 4);
        }
    }
}
