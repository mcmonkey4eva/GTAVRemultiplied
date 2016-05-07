using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class SetIPLDataPacketOut : AbstractPacketOut
    {
        public SetIPLDataPacketOut()
        {
            ID = ServerToClientPacket.SET_IPL_DATA;
            Data = GTAVUtilities.GetIPLData();
        }
    }
}
