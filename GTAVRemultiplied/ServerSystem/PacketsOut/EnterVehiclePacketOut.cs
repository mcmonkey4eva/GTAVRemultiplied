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
        public EnterVehiclePacketOut(Ped ped, Vehicle veh, VehicleSeat seat)
        {
            ID = ServerToClientPacket.ENTER_VEHICLE;
            Data = new byte[4 + 1 + 4];
            BitConverter.GetBytes(veh.Handle).CopyTo(Data, 0);
            Data[4] = (byte)(seat + 3);
            BitConverter.GetBytes(ped.Handle).CopyTo(Data, 4 + 1);
        }
    }
}
