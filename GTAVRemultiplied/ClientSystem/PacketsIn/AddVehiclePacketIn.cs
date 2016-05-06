using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class AddVehiclePacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 24)
            {
                return false;
            }
            int id = BitConverter.ToInt32(data, 0);
            VehicleHash hash = (VehicleHash)BitConverter.ToInt32(data, 4);
            Vector3 location = new Vector3();
            location.X = BitConverter.ToSingle(data, 4 + 4);
            location.Y = BitConverter.ToSingle(data, 4 + 4 + 4);
            location.Z = BitConverter.ToSingle(data, 4 + 4 + 4 + 4);
            float heading = BitConverter.ToSingle(data, 4 + 4 + 4 + 4 + 4);
            Vehicle vehicle = World.CreateVehicle(new Model(hash), location, heading);
            ClientConnectionScript.ServerAddedVehicles[id] = vehicle;
            return true;
        }
    }
}
