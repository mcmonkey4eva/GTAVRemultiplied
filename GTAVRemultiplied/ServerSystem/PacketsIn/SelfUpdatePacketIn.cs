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
            Vector3 vel = new Vector3();
            vel.X = BitConverter.ToSingle(data, 16 + 12 + 4);
            vel.Y = BitConverter.ToSingle(data, 16 + 12 + 4 + 4);
            vel.Z = BitConverter.ToSingle(data, 16 + 12 + 4 + 8);
            WeaponHash weap = (WeaponHash)BitConverter.ToUInt32(data, 16 + 12);
            PedFlags flags = (PedFlags)data[16 + 12 + 4 + 12 + 12];
            if (aim.LengthSquared() > 0.1)
            {
                client.Character.Task.AimAt(client.Character.Position + aim * 50, 1000);
            }
            client.lGoal = vec;
            client.speed = vel.Length();
            if (client.Character.Weapons.Current.Hash != weap)
            {
                client.Character.Weapons.Give(weap, 1000, true, true);
                client.Character.Weapons.Select(weap);
            }
            if (client.Character.CurrentVehicle != null && client.InVehicle)
            {
                client.Character.CurrentVehicle.PositionNoOffset = vec;
            }
            if (client.Character.CurrentVehicle != null && client.InVehicle)
            {
                client.Character.CurrentVehicle.Velocity = vel;
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
