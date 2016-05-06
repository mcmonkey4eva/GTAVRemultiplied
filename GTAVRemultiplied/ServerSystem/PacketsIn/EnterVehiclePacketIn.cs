using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAVRemultiplied.ServerSystem.PacketsIn
{
    public class EnterVehiclePacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(GTAVServerClientConnection client, byte[] data)
        {
            if (data.Length != 5)
            {
                return false;
            }
            int veh = BitConverter.ToInt32(data, 0);
            // TODO: Validate vehicle ID!
            VehicleSeat seat = (VehicleSeat)(data[4] - 3);
            client.Character.SetIntoVehicle(new Vehicle(veh), seat);
            return true;
        }
    }
}
