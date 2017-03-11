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
            Vector3 aim = new Vector3(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8));
            Ped ped = new Ped(ClientConnectionScript.ServerToClientPed[BitConverter.ToInt32(data, 12)]);
            Vector3 ori = ped.Weapons.CurrentWeaponObject.Position;
            Vector3 vec = ori + aim * 50;
            // SET_PED_SHOOTS_AT_COORD(Ped ped, float x, float y, float z, BOOL toggle)
            Function.Call(Hash.SET_PED_SHOOTS_AT_COORD, ped.Handle, vec.X, vec.Y, vec.Z, true);
            /*void SHOOT_SINGLE_BULLET_BETWEEN_COORDS(float x1, float y1, float z1, float x2, float y2, float z2, int damage, BOOL p7, Hash weaponHash,
             Ped ownerPed, BOOL isAudible, BOOL isInvisible, float speed)*/
            Function.Call(Hash.SHOOT_SINGLE_BULLET_BETWEEN_COORDS, ori.X, ori.Y, ori.Z, vec.X, vec.Y, vec.Z, 1-0, true, ped.Weapons.Current.Hash, ped, true, false, -1);
            return true;
        }
    }
}
