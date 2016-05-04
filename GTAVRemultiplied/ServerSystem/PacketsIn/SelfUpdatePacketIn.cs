using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;

namespace GTAVRemultiplied.ServerSystem.PacketsIn
{
    public class SelfUpdatePacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(GTAVServerClientConnection client, byte[] data)
        {
            if (data.Length != 16)
            {
                return false;
            }
            Vector3 vec = new Vector3();
            vec.X = BitConverter.ToSingle(data, 0);
            vec.Y = BitConverter.ToSingle(data, 4);
            vec.Z = BitConverter.ToSingle(data, 8);
            client.Character.PositionNoOffset = vec;
            client.Character.Heading = BitConverter.ToSingle(data, 12);
            return true;
        }
    }
}
