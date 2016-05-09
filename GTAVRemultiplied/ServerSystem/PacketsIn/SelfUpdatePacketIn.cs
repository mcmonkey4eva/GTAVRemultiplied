using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVRemultiplied.ServerSystem.PacketsIn
{
    public class SelfUpdatePacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(GTAVServerClientConnection client, byte[] data)
        {
            if (data.Length != 16 + 12 + 4 + 12 + 12 + 1)
            {
                return false;
            }
            Vector3 vec = new Vector3();
            vec.X = BitConverter.ToSingle(data, 0);
            vec.Y = BitConverter.ToSingle(data, 4);
            vec.Z = BitConverter.ToSingle(data, 8);
            float head = BitConverter.ToSingle(data, 12);
            if (client.Character.CurrentVehicle != null && client.InVehicle)
            {
                client.Character.CurrentVehicle.Heading = head;
            }
            else
            {
                client.Character.Heading = head;
            }
            float dist = vec.DistanceToSquared2D(client.Character.Position);
            Vector3 aim = new Vector3();
            aim.X = BitConverter.ToSingle(data, 16);
            aim.Y = BitConverter.ToSingle(data, 16 + 4);
            aim.Z = BitConverter.ToSingle(data, 16 + 8);
            client.Aim = aim;
            WeaponHash weap = (WeaponHash)BitConverter.ToUInt32(data, 16 + 12);
            byte flags = data[16 + 12 + 4 + 12 + 12];
            if (dist > 10f)
            {
                if (aim.LengthSquared() > 0.1)
                {
                    client.Character.Task.AimAt(client.Character.Position + aim * 50, 1000);
                }
                client.Character.PositionNoOffset = vec;
            }
            else if (dist < 0.25f)
            {
                if (aim.LengthSquared() > 0.1)
                {
                    client.Character.Task.AimAt(client.Character.Position + aim * 50, 1000);
                }
            }
            else
            {
                if (!client.Character.IsJumping)
                {
                    bool isRunning = (flags & 1) == 1;
                    bool isSprinting = (flags & 2) == 2;
                    float speed = isSprinting ? 5.0f : isRunning ? 4.0f : 1.0f;
                    // void TASK_GO_STRAIGHT_TO_COORD(Ped ped, float x, float y, float z, float speed, int timeout, float targetHeading, float distanceToSlide)
                    Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, client.Character.Handle, vec.X, vec.Y, vec.Z, speed, -1, 0.0f, 0.0f);
                }
                if (aim.LengthSquared() > 0.1)
                {
                    Vector3 taim = client.Character.Position + aim * 50;
                    //TASK_GO_TO_COORD_WHILE_AIMING_AT_COORD(Ped ped, float x, float y, float z, float aimAtX, float aimAtY, float aimAtZ, float moveSpeed, BOOL p8, float p9, float p10, BOOL p11, Any flags, BOOL p13, Hash firingPattern)
                    Function.Call(Hash.TASK_GO_TO_COORD_WHILE_AIMING_AT_COORD, client.Character.Handle, vec.X, vec.Y, vec.Z, taim.X, taim.Y, taim.Z, 1f, false, -1f, -1f, false, 0, false, (int)FiringPattern.DelayFireByOneSec);
                }
            }
            if (client.Character.Weapons.Current.Hash != weap)
            {
                client.Character.Weapons.Give(weap, 1000, true, true);
                client.Character.Weapons.Select(weap);
            }
            if (client.Character.CurrentVehicle != null && client.InVehicle)
            {
                client.Character.CurrentVehicle.PositionNoOffset = vec;
            }
            Vector3 vel = new Vector3();
            vel.X = BitConverter.ToSingle(data, 16 + 12 + 4);
            vel.Y = BitConverter.ToSingle(data, 16 + 12 + 4 + 4);
            vel.Z = BitConverter.ToSingle(data, 16 + 12 + 4 + 8);
            if (client.Character.CurrentVehicle != null && client.InVehicle)
            {
                client.Character.CurrentVehicle.Velocity = vel;
            }
            else
            {
                client.Character.Velocity = vel;
            }
            Vector3 rot = new Vector3();
            rot.X = BitConverter.ToSingle(data, 16 + 12 + 4 + 12);
            rot.Y = BitConverter.ToSingle(data, 16 + 12 + 4 + 12 + 4);
            rot.Z = BitConverter.ToSingle(data, 16 + 12 + 4 + 12 + 8);
            if (client.Character.CurrentVehicle != null && client.InVehicle)
            {
                client.Character.CurrentVehicle.Rotation = rot;
            }
            return true;
        }
    }
}
