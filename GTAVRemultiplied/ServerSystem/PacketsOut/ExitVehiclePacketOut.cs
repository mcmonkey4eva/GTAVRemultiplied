using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class ExitVehiclePacketOut : AbstractPacketOut
    {
        public ExitVehiclePacketOut() // TODO: RMP Player object!
        {
            ID = ServerToClientPacket.EXIT_VEHICLE;
            Data = new byte[0];
        }
    }
}
