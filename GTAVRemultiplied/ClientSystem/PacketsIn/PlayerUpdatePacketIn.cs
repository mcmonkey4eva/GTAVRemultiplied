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
    public class PlayerUpdatePacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 16 + 12 + 4 + 12 + 4 + 1)
            {
                return false;
            }
            int serverPed = BitConverter.ToInt32(data, 16 + 12 + 4 + 12);
            Ped ped = new Ped(ClientConnectionScript.ServerToClientPed[serverPed]);
            Vector3 vec = new Vector3();
            vec.X = BitConverter.ToSingle(data, 0);
            vec.Y = BitConverter.ToSingle(data, 4);
            vec.Z = BitConverter.ToSingle(data, 8);
            ClientConnectionScript.ServerPedKnownPosition[serverPed].WorldPos = vec;
            ped.Heading = BitConverter.ToSingle(data, 12);
            float dist = vec.DistanceToSquared2D(ped.Position);
            Vector3 aim = new Vector3();
            aim.X = BitConverter.ToSingle(data, 16);
            aim.Y = BitConverter.ToSingle(data, 16 + 4);
            aim.Z = BitConverter.ToSingle(data, 16 + 8);
            WeaponHash weap = (WeaponHash)BitConverter.ToUInt32(data, 16 + 12);
            byte flags = data[16 + 12 + 4 + 12 + 4];
            if (dist > 10f)
            {
                if (aim.LengthSquared() > 0.1)
                {
                    ped.Task.AimAt(ped.Position + aim * 50, 1000);
                }
                ped.PositionNoOffset = vec;
            }
            else if (dist < 0.25f)
            {
                if (aim.LengthSquared() > 0.1)
                {
                    ped.Task.AimAt(ped.Position + aim * 50, 1000);
                }
            }
            else
            {
                if (!ped.IsJumping)
                {
                    bool isRunning = (flags & 2) == 1;
                    bool isSprinting = (flags & 4) == 4;
                    float speed = isSprinting ? 5.0f : isRunning ? 4.0f : 1.0f;
                    // void TASK_GO_STRAIGHT_TO_COORD(Ped ped, float x, float y, float z, float speed, int timeout, float targetHeading, float distanceToSlide)
                    Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, ped.Handle, vec.X, vec.Y, vec.Z, speed, -1, 0.0f, 0.0f);
                }
                if (aim.LengthSquared() > 0.1)
                {
                    Vector3 taim = ped.Position + aim * 50;
                    //TASK_GO_TO_COORD_WHILE_AIMING_AT_COORD(Ped ped, float x, float y, float z, float aimAtX, float aimAtY, float aimAtZ, float moveSpeed, BOOL p8, float p9, float p10, BOOL p11, Any flags, BOOL p13, Hash firingPattern)
                    Function.Call(Hash.TASK_GO_TO_COORD_WHILE_AIMING_AT_COORD, ped.Handle, vec.X, vec.Y, vec.Z, taim.X, taim.Y, taim.Z, 1f, false, -1f, -1f, false, 0, false, (int)FiringPattern.DelayFireByOneSec);
                }
            }
            if (ped.Weapons.Current.Hash != weap)
            {
                ped.Weapons.Give(weap, 1000, true, true);
                ped.Weapons.Select(weap);
            }
            Vector3 vel = new Vector3();
            vel.X = BitConverter.ToSingle(data, 16 + 12 + 4);
            vel.Y = BitConverter.ToSingle(data, 16 + 12 + 4 + 4);
            vel.Z = BitConverter.ToSingle(data, 16 + 12 + 4 + 8);
            ped.Velocity = vel;
            bool isDead = (flags & 1) == 1;
            if (isDead && !ped.IsDead)
            {
                ped.IsInvincible = false;
                ped.Kill();
            }
            else if (!isDead && ped.IsDead)
            {
                ped.Health = 1;
                ped.IsInvincible = true;
            }
            return true;
        }
    }
}
