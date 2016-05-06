using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (ClientConnectionScript.ServerAddedVehicles.ContainsKey(id))
            {
                Vehicle vehicle = ClientConnectionScript.ServerAddedVehicles[id];
                vehicle.Delete();
                ClientConnectionScript.ServerAddedVehicles.Remove(id);
            }
            return true;
        }
    }
}
