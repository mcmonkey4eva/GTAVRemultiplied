using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ClientSystem.PacketsOut
{
    public class ExitVehiclePacketOut : AbstractPacketOut
    {
        public ExitVehiclePacketOut()
        {
            ID = ClientToServerPacket.EXIT_VEHICLE;
            Data = new byte[0];
        }
    }
}
