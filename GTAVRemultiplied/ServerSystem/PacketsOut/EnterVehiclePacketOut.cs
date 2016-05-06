using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class EnterVehiclePacketOut : AbstractPacketOut
    {
        public EnterVehiclePacketOut(Vehicle veh, VehicleSeat seat) // TODO: RMP Player object!
        {
            ID = ServerToClientPacket.ENTER_VEHICLE;
            Data = new byte[4 + 1];
            BitConverter.GetBytes(veh.Handle).CopyTo(Data, 0);
            Data[4] = (byte)(seat + 3);
        }
    }
}
