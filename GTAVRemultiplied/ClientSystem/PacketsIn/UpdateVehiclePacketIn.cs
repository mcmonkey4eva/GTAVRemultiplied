﻿using System;
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
            if (data.Length != 4 + 12 + 12 + 4 + 1)
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
            float heading = BitConverter.ToSingle(data, 4 + 12 + 12);
            byte flags = data[4 + 12 + 12 + 4];
            bool isDead = (flags & 1) == 1;
            int veh;
            if (ClientConnectionScript.ServerToClientVehicle.TryGetValue(id, out veh))
            {
                Vehicle vehicle = new Vehicle(veh);
                vehicle.PositionNoOffset = pos;
                vehicle.Velocity = vel;
                vehicle.Heading = heading;
                if (isDead && !vehicle.IsDead)
                {
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