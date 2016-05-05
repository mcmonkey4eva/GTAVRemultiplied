using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ClientSystem.PacketsOut
{
    public class JumpPacketOut : AbstractPacketOut
    {
        public JumpPacketOut()
        {
            ID = ClientToServerPacket.JUMP;
            Data = new byte[0];
        }
    }
}
