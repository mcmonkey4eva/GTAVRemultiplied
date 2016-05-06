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
            int veh;
            if (ClientConnectionScript.ServerToClientVehicle.TryGetValue(id, out veh))
            {
                Vehicle vehicle = new Vehicle(veh);
                vehicle.Delete();
                ClientConnectionScript.ServerToClientVehicle.Remove(id);
                ClientConnectionScript.ClientToServerVehicle.Remove(veh);
            }
            else
            {
                Log.Message("Warning", "Unknown vehicle removed!", 'Y');
            }
            return true;
        }
    }
}
