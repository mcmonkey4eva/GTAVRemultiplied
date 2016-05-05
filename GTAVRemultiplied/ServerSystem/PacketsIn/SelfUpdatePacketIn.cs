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
            if (data.Length != 16 + 12 + 4 + 12)
            {
                return false;
            }
            Vector3 vec = new Vector3();
            vec.X = BitConverter.ToSingle(data, 0);
            vec.Y = BitConverter.ToSingle(data, 4);
            vec.Z = BitConverter.ToSingle(data, 8);
            client.Character.Heading = BitConverter.ToSingle(data, 12);
            float dist = vec.DistanceToSquared2D(client.Character.Position);
            Vector3 aim = new Vector3();
            aim.X = BitConverter.ToSingle(data, 16);
            aim.Y = BitConverter.ToSingle(data, 16 + 4);
            aim.Z = BitConverter.ToSingle(data, 16 + 8);
            WeaponHash weap = (WeaponHash)BitConverter.ToUInt32(data, 16 + 12);
            client.Character.Task.ClearAll();
            if (dist > 10f)
            {
                client.Character.Task.StandStill(1000);
                if (aim.LengthSquared() > 0.1)
                {
                    client.Character.Task.AimAt(client.Character.Position + aim * 20, 1000);
                }
                client.Character.PositionNoOffset = vec;
            }
            else if (dist < 0.7f)
            {
                client.Character.Task.StandStill(1000);
                if (aim.LengthSquared() > 0.1)
                {
                    client.Character.Task.AimAt(client.Character.Position + aim * 20, 1000);
                }
            }
            else
            {
                client.Character.Task.GoTo(vec, true);
                if (aim.LengthSquared() > 0.1)
                {
                    Vector3 taim = client.Character.Position + aim * 20;
                    //TASK_GO_TO_COORD_WHILE_AIMING_AT_COORD(Ped ped, float x, float y, float z, float aimAtX, float aimAtY, float aimAtZ, float moveSpeed, BOOL p8, float p9, float p10, BOOL p11, Any flags, BOOL p13, Hash firingPattern)
                    Function.Call(Hash.TASK_GO_TO_COORD_WHILE_AIMING_AT_COORD, client.Character.Handle, vec.X, vec.Y, vec.Z, taim.X, taim.Y, taim.Z, 1f, false, -1f, -1f, false, 0, false, (int)FiringPattern.DelayFireByOneSec);
                }
            }
            if (client.Character.Weapons.Current.Hash != weap)
            {
                client.Character.Weapons.Give(weap, 1000, true, true);
                client.Character.Weapons.Select(weap);
            }
            Vector3 vel = new Vector3();
            vel.X = BitConverter.ToSingle(data, 16 + 12 + 4);
            vel.Y = BitConverter.ToSingle(data, 16 + 12 + 4 + 4);
            vel.Z = BitConverter.ToSingle(data, 16 + 12 + 4 + 8);
            client.Character.Velocity = vel;
            return true;
        }
    }
}
