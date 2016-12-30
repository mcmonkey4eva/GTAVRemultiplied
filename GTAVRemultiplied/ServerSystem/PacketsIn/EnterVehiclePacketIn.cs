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
            // TODO: Validate the seat!
            // TODO: Make sure we're not forcing anyone out of their seat!
            // TODO: Make sure the character can validly move into this vehicle, and isn't using vehicles to teleport!
            VehicleSeat seat = (VehicleSeat)(data[4] - 3);
            client.Character.SetIntoVehicle(new Vehicle(veh), seat);
            client.InVehicle = true;
            client.lRot = new Vehicle(veh).Quaternion;
            client.lPos = new Vehicle(veh).Position;
            return true;
        }
    }
}
