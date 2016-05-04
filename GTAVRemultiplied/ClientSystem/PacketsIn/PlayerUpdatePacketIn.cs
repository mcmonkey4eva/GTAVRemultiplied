using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class PlayerUpdatePacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 16)
            {
                return false;
            }
            Vector3 vec = new Vector3();
            vec.X = BitConverter.ToSingle(data, 0);
            vec.Y = BitConverter.ToSingle(data, 4);
            vec.Z = BitConverter.ToSingle(data, 8);
            ClientConnectionScript.Character.PositionNoOffset = vec;
            ClientConnectionScript.Character.Heading = BitConverter.ToSingle(data, 12);
            return true;
        }
    }
}
