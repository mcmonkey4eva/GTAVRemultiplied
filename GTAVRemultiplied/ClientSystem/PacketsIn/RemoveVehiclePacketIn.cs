using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class RemoveVehiclePacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 4)
            {
                return false;
            }
            int id = BitConverter.ToInt32(data, 0);
            if (ClientConnectionScript.ServerToClientVehicle.TryGetValue(id, out int veh))
            {
                Vehicle vehicle = new Vehicle(veh);
                vehicle.Delete();
                ClientConnectionScript.ServerToClientVehicle.Remove(id);
                ClientConnectionScript.ClientToServerVehicle.Remove(veh);
                ClientConnectionScript.ServerVehKnownPosition.Remove(id);
            }
            return true;
        }
    }
}
