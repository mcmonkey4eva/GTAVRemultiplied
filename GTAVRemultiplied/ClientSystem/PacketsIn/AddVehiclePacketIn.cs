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
            int ind = 4 + 4 + 12 + 4 + 8 + 2 + 4;
            if (data.Length != ind + 5)
            {
                return false;
            }
            int id = BitConverter.ToInt32(data, 0);
            int hash = BitConverter.ToInt32(data, 4);
            Vector3 pos = new Vector3();
            pos.X = BitConverter.ToSingle(data, 4 + 4);
            pos.Y = BitConverter.ToSingle(data, 4 + 4 + 4);
            pos.Z = BitConverter.ToSingle(data, 4 + 4 + 8);
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
            vehicle.NumberPlate = numPlate.Trim();
            vehicle.NumberPlateType = (NumberPlateType)data[4 + 4 + 12 + 4 + 8];
            vehicle.ColorCombination = BitConverter.ToInt32(data, 4 + 4 + 12 + 4 + 8 + 2);
            vehicle.PrimaryColor = (VehicleColor)data[ind];
            vehicle.SecondaryColor = (VehicleColor)data[ind + 1];
            vehicle.PearlescentColor = (VehicleColor)data[ind + 2];
            vehicle.TrimColor = (VehicleColor)data[ind + 3];
            vehicle.RimColor = (VehicleColor)data[ind + 4];
            ClientConnectionScript.ServerToClientVehicle[id] = vehicle.Handle;
            ClientConnectionScript.ClientToServerVehicle[vehicle.Handle] = id;
            return true;
        }
    }
}
