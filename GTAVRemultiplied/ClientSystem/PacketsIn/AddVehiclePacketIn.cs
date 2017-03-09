using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class AddVehiclePacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            int ind = 4 + 4 + 12 + 4 + 8 + 1 + 4;
            if (data.Length != ind + 5)
            {
                return false;
            }
            int id = BitConverter.ToInt32(data, 0);
            if (ClientConnectionScript.ServerToClientVehicle.ContainsKey(id))
            {
                return true;
            }
            int hash = BitConverter.ToInt32(data, 4);
            Vector3 pos = new Vector3(BitConverter.ToSingle(data, 4 + 4), BitConverter.ToSingle(data, 4 + 4 + 4), BitConverter.ToSingle(data, 4 + 4 + 8));
            float heading = BitConverter.ToSingle(data, 4 + 4 + 12);
            Vehicle vehicle = World.CreateVehicle(new Model(hash), pos, heading);
            if (vehicle == null)
            {
                vehicle = World.CreateVehicle(new Model(hash), Game.Player.Character.Position + new Vector3(10, 10, 10), heading);
                if (vehicle == null)
                {
                    Log.Message("Warning", "Null vehicle spawned: " + hash, 'Y');
                    return true;
                }
            }
            vehicle.IsPersistent = true;
            vehicle.IsInvincible = true;
            string numPlate = "";
            for (int i = 0; i < 8; i++)
            {
                numPlate += (char)data[4 + 4 + 12 + 4 + i];
            }
            vehicle.Mods.LicensePlate = numPlate.Trim();
            vehicle.Mods.LicensePlateStyle = (LicensePlateStyle)data[4 + 4 + 12 + 4 + 8];
            vehicle.Mods.ColorCombination = BitConverter.ToInt32(data, 4 + 4 + 12 + 4 + 8 + 1);
            vehicle.Mods.PrimaryColor = (VehicleColor)data[ind];
            vehicle.Mods.SecondaryColor = (VehicleColor)data[ind + 1];
            vehicle.Mods.PearlescentColor = (VehicleColor)data[ind + 2];
            vehicle.Mods.TrimColor = (VehicleColor)data[ind + 3];
            vehicle.Mods.RimColor = (VehicleColor)data[ind + 4];
            ClientConnectionScript.ServerToClientVehicle[id] = vehicle.Handle;
            ClientConnectionScript.ClientToServerVehicle[vehicle.Handle] = id;
            VehicleInfo inf = new VehicleInfo()
            {
                lPos = vehicle.Position,
                lGoal = vehicle.Position,
                vehicle = vehicle,
                lRot = vehicle.Quaternion,
                lRotGoal = vehicle.Quaternion
            };
            ClientConnectionScript.ServerVehKnownPosition[id] = inf;
            return true;
        }
    }
}
