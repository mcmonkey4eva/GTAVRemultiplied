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
            if (data.Length != 4 + 4 + 12 + 4)
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
            if (vehicle != null)
            {
                vehicle.IsPersistent = true;
                vehicle.IsInvincible = true;
                ClientConnectionScript.ServerToClientVehicle[id] = vehicle.Handle;
                ClientConnectionScript.ClientToServerVehicle[vehicle.Handle] = id;
            }
            else
            {
                vehicle = World.CreateVehicle(new Model(hash), Game.Player.Character.Position + new Vector3(10, 10, 10), heading);
                if (vehicle != null)
                {
                    vehicle.IsPersistent = true;
                    vehicle.IsInvincible = true;
                    ClientConnectionScript.ServerToClientVehicle[id] = vehicle.Handle;
                    ClientConnectionScript.ClientToServerVehicle[vehicle.Handle] = id;
                }
                else
                {
                    Log.Message("Warning", "Null vehicle spawned: " + hash, 'Y');
                }
            }
            return true;
        }
    }
}
