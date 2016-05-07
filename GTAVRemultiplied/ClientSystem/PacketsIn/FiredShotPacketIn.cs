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
    public class FiredShotPacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 12 + 4)
            {
                return false;
            }
            Vector3 aim = new Vector3();
            aim.X = BitConverter.ToSingle(data, 0);
            aim.Y = BitConverter.ToSingle(data, 4);
            aim.Z = BitConverter.ToSingle(data, 8);
            Ped ped = new Ped(ClientConnectionScript.ServerToClientPed[BitConverter.ToInt32(data, 12)]);
            Vector3 vec = ped.Position + aim * 50;
            // SET_PED_SHOOTS_AT_COORD(Ped ped, float x, float y, float z, BOOL toggle)
            Function.Call(Hash.SET_PED_SHOOTS_AT_COORD, ped.Handle, vec.X, vec.Y, vec.Z, true);
            return true;
        }
    }
}
