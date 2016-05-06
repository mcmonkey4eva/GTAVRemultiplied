using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAVRemultiplied.ClientSystem.PacketsOut
{
    public class EnterVehiclePacketOut : AbstractPacketOut
    {
        public EnterVehiclePacketOut(Vehicle veh, VehicleSeat seat)
        {
            ID = ClientToServerPacket.ENTER_VEHICLE;
            Data = new byte[4 + 1];
            BitConverter.GetBytes(ClientConnectionScript.ClientToServerVehicle[veh.Handle]).CopyTo(Data, 0);
            Data[4] = (byte)(seat + 3);
        }
    }
}
