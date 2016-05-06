using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class RemoveVehiclePacketOut : AbstractPacketOut
    {
        public RemoveVehiclePacketOut(int id)
        {
            ID = ServerToClientPacket.REMOVE_VEHICLE;
            Data = BitConverter.GetBytes(id);
        }
    }
}
