using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVRemultiplied
{
    public static class GTAVExtensions
    {
        public static Vector3 GetBoneCoord(this Ped ped, Bone bone)
        {
            return Function.Call<Vector3>(Hash.GET_PED_BONE_COORDS, ped.Handle, ped.Bones[bone].Index, 0f, 0f, 0f);
        }
    }
}
