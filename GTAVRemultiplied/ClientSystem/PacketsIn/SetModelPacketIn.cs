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
    public class SetModelPacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 4 + 4)
            {
                return false;
            }
            Ped ped = new Ped(ClientConnectionScript.ServerToClientPed[BitConverter.ToInt32(data, 4)]);
            Model mod = new Model(BitConverter.ToInt32(data, 0));
            Vector3 pos = ped.Position;
            float heading = ped.Heading;
            ped.Delete();
            ped = World.CreatePed(mod, pos, heading);
            if (ped != null)
            {
                ped.IsPersistent = true;
                ped.IsInvincible = true;
            }
            else
            {
                Log.Message("Warning", "Null character spawned: " + mod.Hash, 'Y');
            }
            return true;
        }
    }
}
