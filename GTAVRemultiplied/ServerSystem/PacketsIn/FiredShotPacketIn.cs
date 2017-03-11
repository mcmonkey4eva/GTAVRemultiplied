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
    public class FiredShotPacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(GTAVServerClientConnection client, byte[] data)
        {
            if (data.Length != 12)
            {
                return false;
            }
            Vector3 aim = new Vector3(BitConverter.ToSingle(data, 0), BitConverter.ToSingle(data, 4), BitConverter.ToSingle(data, 8));
            Vector3 ori = client.Character.Weapons.CurrentWeaponObject.Position;
            Vector3 vec = ori + aim * 50;
            client.lastShotAim = aim;
            // SET_PED_SHOOTS_AT_COORD(Ped ped, float x, float y, float z, BOOL toggle)
            //Function.Call(Hash.SET_PED_SHOOTS_AT_COORD, client.Character.Handle, vec.X, vec.Y, vec.Z, true);
            /*void SHOOT_SINGLE_BULLET_BETWEEN_COORDS(float x1, float y1, float z1, float x2, float y2, float z2, int damage, BOOL p7, Hash weaponHash,
             Ped ownerPed, BOOL isAudible, BOOL isInvisible, float speed)*/
            Function.Call(Hash.SHOOT_SINGLE_BULLET_BETWEEN_COORDS, ori.X, ori.Y, ori.Z, vec.X, vec.Y, vec.Z, 10, true, client.Character.Weapons.Current.Hash, client.Character, true, false, -1);
            return true;
        }
    }
}
