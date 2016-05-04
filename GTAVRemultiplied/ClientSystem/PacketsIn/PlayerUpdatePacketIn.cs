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
            ClientConnectionScript.Character.Heading = BitConverter.ToSingle(data, 12);
            float dist = vec.DistanceToSquared2D(ClientConnectionScript.Character.Position);
            if (dist > 10f)
            {
                ClientConnectionScript.Character.Task.StandStill(1000);
                ClientConnectionScript.Character.PositionNoOffset = vec;
            }
            else if (dist < 0.7f)
            {
                ClientConnectionScript.Character.Task.StandStill(1000);
            }
            else
            {
                ClientConnectionScript.Character.Task.GoTo(vec, false);
            }
            return true;
        }
    }
}
