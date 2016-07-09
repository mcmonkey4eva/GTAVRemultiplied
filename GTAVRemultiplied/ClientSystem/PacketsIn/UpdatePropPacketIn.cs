using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class UpdatePropPacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 4 + 12 + 12 + 12 + 1 + 12 + 4)
            {
                return false;
            }
            int id = BitConverter.ToInt32(data, 0);
            Vector3 pos = new Vector3();
            pos.X = BitConverter.ToSingle(data, 4);
            pos.Y = BitConverter.ToSingle(data, 4 + 4);
            pos.Z = BitConverter.ToSingle(data, 4 + 8);
            Vector3 vel;
            vel.X = BitConverter.ToSingle(data, 4 + 12);
            vel.Y = BitConverter.ToSingle(data, 4 + 12 + 4);
            vel.Z = BitConverter.ToSingle(data, 4 + 12 + 8);
            Quaternion rot;
            rot.X = BitConverter.ToSingle(data, 4 + 12 + 12);
            rot.Y = BitConverter.ToSingle(data, 4 + 12 + 12 + 4);
            rot.Z = BitConverter.ToSingle(data, 4 + 12 + 12 + 8);
            Vector3 rotvel;
            rotvel.X = BitConverter.ToSingle(data, 4 + 12 + 12 + 12 + 1);
            rotvel.Y = BitConverter.ToSingle(data, 4 + 12 + 12 + 12 + 1 + 4);
            rotvel.Z = BitConverter.ToSingle(data, 4 + 12 + 12 + 12 + 1 + 8);
            rot.W = BitConverter.ToSingle(data, 4 + 12 + 12 + 12 + 1 + 12);
            byte flags = data[4 + 12 + 12 + 12];
            bool isDead = (flags & 1) == 1;
            int prp;
            if (ClientConnectionScript.ServerToClientProp.TryGetValue(id, out prp))
            {
                Prop prop = new Prop(prp);
                if (!prop.Exists())
                {
                    Log.Message("Warning", "Prop disappeared: " + prp, 'Y');
                    return true;
                }
                prop.PositionNoOffset = pos;
                //prop.Velocity = vel;
                prop.Quaternion = rot;
                /*GTAVUtilities.SetRotationVelocity(prop, rotvel);
                if (isDead && !prop.IsDead)
                {
                    prop.IsInvincible = false;
                    prop.Health = -1;
                    if (!prop.IsDead)
                    {
                        prop.Health = 0;
                    }
                }
                else if (!isDead && prop.IsDead)
                {
                    prop.IsInvincible = true;
                    prop.Health = 1;
                }*/
            }
            else
            {
                for (int i = 0; i < PropSpawnScript.propsToSpawn.Length; i++)
                {
                    if (PropSpawnScript.propsToSpawn[i].Item3 == id)
                    {
                        return true;
                    }
                }
                Log.Message("Warning", "Unknown prop updated!", 'Y');
            }
            return true;
        }
    }
}
