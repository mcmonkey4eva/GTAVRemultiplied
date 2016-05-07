using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class SetModelPacketOut : AbstractPacketOut
    {
        public SetModelPacketOut(int modhash) // TODO: RMP Player object
        {
            ID = ServerToClientPacket.SET_MODEL;
            Data = BitConverter.GetBytes(modhash);
        }
    }
}
