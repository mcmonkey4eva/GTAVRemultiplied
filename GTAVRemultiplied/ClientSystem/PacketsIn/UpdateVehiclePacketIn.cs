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
            int ind = 4 + 12 + 12 + 12 + 1 + 12 + 4;
            if (data.Length != ind + 4)
            {
                return false;
            }
            int id = BitConverter.ToInt32(data, 0);
            Vector3 pos = new Vector3();
            pos.X = BitConverter.ToSingle(data, 4);
            pos.Y = BitConverter.ToSingle(data, 4 + 4);
            pos.Z = BitConverter.ToSingle(data, 4 + 8);
            Vector3 vel = new Vector3();
            vel.X = BitConverter.ToSingle(data, 4 + 12);
            vel.Y = BitConverter.ToSingle(data, 4 + 12 + 4);
            vel.Z = BitConverter.ToSingle(data, 4 + 12 + 8);
            Quaternion rot;
            rot.X = BitConverter.ToSingle(data, 4 + 12 + 12);
            rot.Y = BitConverter.ToSingle(data, 4 + 12 + 12 + 4);
            rot.Z = BitConverter.ToSingle(data, 4 + 12 + 12 + 8);
            Vector3 rotvel = new Vector3();
            rotvel.X = BitConverter.ToSingle(data, 4 + 12 + 12 + 12 + 1);
            rotvel.Y = BitConverter.ToSingle(data, 4 + 12 + 12 + 12 + 1 + 4);
            rotvel.Z = BitConverter.ToSingle(data, 4 + 12 + 12 + 12 + 1 + 8);
            rot.W = BitConverter.ToSingle(data, 4 + 12 + 12 + 12 + 1 + 12);
            VehicleFlags flags = (VehicleFlags)data[4 + 12 + 12 + 12];
            bool isDead = flags.HasFlag(VehicleFlags.DEAD);
            bool siren = flags.HasFlag(VehicleFlags.SIREN_ON);
            bool lights = flags.HasFlag(VehicleFlags.LIGHTS);
            bool lights_search = flags.HasFlag(VehicleFlags.SEARCH_LIGHTS);
            bool lights_int = flags.HasFlag(VehicleFlags.INTERIOR_LIGHTS);
            bool lights_taxi = flags.HasFlag(VehicleFlags.TAXI_LIGHTS);
            float speed = BitConverter.ToSingle(data, ind);
            int veh;
            if (ClientConnectionScript.ServerToClientVehicle.TryGetValue(id, out veh))
            {
                Vehicle vehicle = new Vehicle(veh);
                Vehicle cveh = Game.Player.Character.CurrentVehicle;
                if (cveh == null || cveh.Handle != vehicle.Handle)
                {
                    vehicle.PositionNoOffset = pos;
                    vehicle.Velocity = vel;
                    vehicle.Quaternion = rot;
                    vehicle.Speed = speed;
                    vehicle.IsSirenActive = siren;
                    vehicle.AreLightsOn = lights;
                    vehicle.IsSearchLightOn = lights_search;
                    vehicle.IsTaxiLightOn = lights_taxi;
                    vehicle.IsInteriorLightOn = lights_int;
                    // TODO: Find a way to set steering angle? Perhaps use driver AI magic?
                    GTAVUtilities.SetRotationVelocity(vehicle, rotvel);
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
                else if (vehicle.IsDead && !isDead)
                {
                    vehicle.IsInvincible = true;
                    vehicle.Repair();
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
