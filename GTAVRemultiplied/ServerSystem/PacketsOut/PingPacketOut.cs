using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class PingPacketOut : AbstractPacketOut
    {
        public PingPacketOut(byte code)
        {
            ID = ServerToClientPacket.PING;
            Data = new byte[] { code };
        }
    }
}
