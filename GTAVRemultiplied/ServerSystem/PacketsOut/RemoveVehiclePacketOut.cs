using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class RemoveVehiclePacketOut : AbstractPacketOut
    {
        public RemoveVehiclePacketOut(int id)
        {
            ID = ServerToClientPacket.REMOVE_VEHICLE;
            Data = new byte[4];
            BitConverter.GetBytes(id).CopyTo(Data, 0);
        }
    }
}
