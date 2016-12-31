using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ClientSystem.PacketsOut
{
    public class PingPacketOut : AbstractPacketOut
    {
        public PingPacketOut(byte code)
        {
            ID = ClientToServerPacket.PING;
            Data = new byte[] { code };
        }
    }
}
