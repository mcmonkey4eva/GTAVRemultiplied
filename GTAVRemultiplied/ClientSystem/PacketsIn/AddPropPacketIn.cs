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
    public class AddPropPacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 4 + 4 + 12)
            {
                return false;
            }
            int id = BitConverter.ToInt32(data, 0);
            int hash = BitConverter.ToInt32(data, 4);
            Vector3 pos = new Vector3();
            pos.X = BitConverter.ToSingle(data, 4 + 4);
            pos.Y = BitConverter.ToSingle(data, 4 + 4 + 4);
            pos.Z = BitConverter.ToSingle(data, 4 + 4 + 8);
            PropSpawnScript.propsToSpawn.Push(new Tuple<int, Vector3, int>(hash, pos, id));
            return true;
        }
    }
}
