using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class UpdateVehiclePacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 4 + 12 + 12 + 12 + 1)
            {
                return false;
            }
            int id = BitConverter.ToInt32(data, 0);
            Vector3 pos;
            pos.X = BitConverter.ToSingle(data, 4);
            pos.Y = BitConverter.ToSingle(data, 4 + 4);
            pos.Z = BitConverter.ToSingle(data, 4 + 8);
            Vector3 vel;
            vel.X = BitConverter.ToSingle(data, 4 + 12);
            vel.Y = BitConverter.ToSingle(data, 4 + 12 + 4);
            vel.Z = BitConverter.ToSingle(data, 4 + 12 + 8);
            Vector3 rot;
            rot.X = BitConverter.ToSingle(data, 4 + 12 + 12);
            rot.Y = BitConverter.ToSingle(data, 4 + 12 + 12 + 4);
            rot.Z = BitConverter.ToSingle(data, 4 + 12 + 12 + 8);
            byte flags = data[4 + 12 + 12 + 12];
            bool isDead = (flags & 1) == 1;
            int veh;
            if (ClientConnectionScript.ServerToClientVehicle.TryGetValue(id, out veh))
            {
                Vehicle vehicle = new Vehicle(veh);
                Vehicle cveh = Game.Player.Character.CurrentVehicle;
                if (cveh == null || cveh.Handle != vehicle.Handle)
                {
                    vehicle.PositionNoOffset = pos;
                    vehicle.Velocity = vel;
                    vehicle.Rotation = rot;
                }
                if (isDead && !vehicle.IsDead)
                {
                    vehicle.IsInvincible = false;
                    vehicle.Explode();
                    if (!vehicle.IsDead)
                    {
                        vehicle.Health = 0;
                    }
                }
            }
            else
            {
                Log.Message("Warning", "Unknown vehicle updated!", 'Y');
            }
            return true;
        }
    }
}
