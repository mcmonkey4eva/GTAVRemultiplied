using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ClientSystem.PacketsOut
{
    public class RequestModelPacketOut : AbstractPacketOut
    {
        public RequestModelPacketOut(int modhash)
        {
            ID = ClientToServerPacket.REQUEST_MODEL;
            Data = BitConverter.GetBytes(modhash);
        }
    }
}
