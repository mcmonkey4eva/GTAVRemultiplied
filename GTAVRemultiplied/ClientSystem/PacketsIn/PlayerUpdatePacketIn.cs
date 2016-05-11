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
            PedInfo pinf = ClientConnectionScript.ServerPedKnownPosition[serverPed];
            pinf.WorldPos = vec;
            ped.Heading = BitConverter.ToSingle(data, 12);
            float dist = vec.DistanceToSquared2D(ped.Position);
            Vector3 aim = new Vector3();
            aim.X = BitConverter.ToSingle(data, 16);
            aim.Y = BitConverter.ToSingle(data, 16 + 4);
            aim.Z = BitConverter.ToSingle(data, 16 + 8);
            WeaponHash weap = (WeaponHash)BitConverter.ToUInt32(data, 16 + 12);
            PedFlags flags = (PedFlags)data[16 + 12 + 4 + 12 + 4];
            if (aim.LengthSquared() > 0.1)
            {
                ped.Task.AimAt(ped.Position + aim * 50, 1000);
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
            pinf.lGoal = vec;
            pinf.speed = vel.Length();
            //ped.Velocity = vel;
            bool isDead = flags.HasFlag(PedFlags.DEAD);
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
