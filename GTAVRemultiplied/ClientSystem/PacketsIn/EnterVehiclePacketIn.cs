using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class EnterVehiclePacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 5)
            {
                return false;
            }
            int veh = BitConverter.ToInt32(data, 0);
            // TODO: Validate vehicle ID!
            VehicleSeat seat = (VehicleSeat)(data[4] - 3);
            ClientConnectionScript.Character.SetIntoVehicle(new Vehicle(ClientConnectionScript.ServerToClientVehicle[veh]), seat);
            return true;
        }
    }
}
