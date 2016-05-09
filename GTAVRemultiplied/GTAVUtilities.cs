using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;
using FreneticScript;
using GTAVRemultiplied.SharedSystems;
using System.IO;
using System.IO.Compression;

namespace GTAVRemultiplied
{
    public class GTAVUtilities
    {
        public static void SwitchCharacter(Model? mod)
        {
            ModelEnforcementScript.WantedModel = mod;
        }

        public static Vector3 StringToVector(string input)
        {
            string[] split = input.SplitFast(',', 3);
            if (split.Length != 3)
            {
                return Vector3.Zero;
            }
            return new Vector3(FreneticScriptUtilities.StringToFloat(split[0]),
                FreneticScriptUtilities.StringToFloat(split[1]),
                FreneticScriptUtilities.StringToFloat(split[2]));
        }

        public static Vector3 GetRotationVelocity(Entity e)
        {
            return Function.Call<Vector3>(Hash.GET_ENTITY_ROTATION_VELOCITY, e.Handle);
        }

        public static void SetRotationVelocity(Entity e, Vector3 rotvel)
        {
            Vector3 rvel = rotvel - GetRotationVelocity(e);
            Function.Call(Hash.APPLY_FORCE_TO_ENTITY, e.Handle, 5, rvel.X, rvel.Y, rvel.Z, 0f, 0f, 0f, 0, 1, 0, 1, 0, 1);
        }

        static string[] ny = new string[]
        {
            "plg_01", "prologue01", "prologue01_lod", "prologue01c", "prologue01c_lod", "prologue01d", "prologue01d_lod", "prologue01e",
            "prologue01e_lod", "prologue01f", "prologue01f_lod", "prologue01g", "prologue01h", "prologue01h_lod", "prologue01i", "prologue01i_lod",
            "prologue01j", "prologue01j_lod", "prologue01k", "prologue01k_lod", "prologue01z", "prologue01z_lod", "plg_02", "prologue02", "prologue02_lod",
            "plg_03", "prologue03", "prologue03_lod", "prologue03b", "prologue03b_lod", "prologue03_grv_dug", "prologue03_grv_dug_lod", "prologue_grv_torch",
            "plg_04", "prologue04", "prologue04_lod", "prologue04b", "prologue04b_lod", "prologue04_cover", "des_protree_end", "des_protree_start",
            "des_protree_start_lod", "plg_05", "prologue05", "prologue05_lod", "prologue05b", "prologue05b_lod", "plg_06", "prologue06", "prologue06_lod",
            "prologue06b", "prologue06b_lod", "prologue06_int", "prologue06_int_lod", "prologue06_pannel", "prologue06_pannel_lod", "prologue_m2_door",
            "prologue_m2_door_lod", "plg_occl_00", "prologue_occl", "plg_rd", "prologuerd", "prologuerdb", "prologuerd_lod"
        };

        static string[] hc = new string[]
        {
            "hei_carrier", "hei_carrier_DistantLights", "hei_Carrier_int1", "hei_Carrier_int2",
            "hei_Carrier_int3", "hei_Carrier_int4", "hei_Carrier_int5", "hei_Carrier_int6", "hei_carrier_LODLights"
        };

        public static void EnableDLC()
        {
            Function.Call(Hash._LOAD_MP_DLC_MAPS);
            Function.Call(Hash._ENABLE_MP_DLC_MAPS, true);
        }

        public static void SpawnNorthYankton()
        {
            foreach (string str in ny)
            {
                Function.Call(Hash.REQUEST_IPL, str);
            }
        }

        public static void SpawnCarrier()
        {
            foreach (string str in hc)
            {
                Function.Call(Hash.REQUEST_IPL, str);
            }
        }
        public static void RemoveNorthYankton()
        {
            foreach (string str in ny)
            {
                Function.Call(Hash.REMOVE_IPL, str);
            }
        }

        public static void RemoveCarrier()
        {
            foreach (string str in hc)
            {
                Function.Call(Hash.REMOVE_IPL, str);
            }
        }

        public static byte[] GetIPLData()
        {
            byte[] data = new byte[IPLList.All_IPLS.Length];
            int i = 0;
            foreach (string str in IPLList.All_IPLS)
            {
                if (Function.Call<bool>(Hash.IS_IPL_ACTIVE, str))
                {
                    data[i] = 1;
                }
                else
                {
                    data[i] = 0;
                }
                i++;
            }
            return data;
        }

        public static void SetIPLData(byte[] data)
        {
            if (data.Length != IPLList.All_IPLS.Length)
            {
                throw new Exception("Invalid IPL data!");
            }
            int i = 0;
            foreach (string str in IPLList.All_IPLS)
            {
                bool isActive = Function.Call<bool>(Hash.IS_IPL_ACTIVE, str);
                if (isActive && data[i] == 0)
                {
                    Function.Call(Hash.REMOVE_IPL, str);
                }
                else if (!isActive && data[i] == 1)
                {
                    Function.Call(Hash.REQUEST_IPL, str);
                }
                i++;
            }
        }

        /// <summary>
        /// Compresses a byte array using the GZip algorithm.
        /// </summary>
        /// <param name="input">Uncompressed data.</param>
        /// <returns>Compressed data.</returns>
        public static byte[] GZip(byte[] input)
        {
            MemoryStream memstream = new MemoryStream();
            GZipStream GZStream = new GZipStream(memstream, CompressionMode.Compress);
            GZStream.Write(input, 0, input.Length);
            GZStream.Close();
            byte[] finaldata = memstream.ToArray();
            memstream.Close();
            return finaldata;
        }

        /// <summary>
        /// Decompress a byte array using the GZip algorithm.
        /// </summary>
        /// <param name="input">Compressed data.</param>
        /// <returns>Uncompressed data.</returns>
        public static byte[] UnGZip(byte[] input)
        {
            using (MemoryStream output = new MemoryStream())
            {
                MemoryStream memstream = new MemoryStream(input);
                GZipStream GZStream = new GZipStream(memstream, CompressionMode.Decompress);
                GZStream.CopyTo(output);
                GZStream.Close();
                memstream.Close();
                return output.ToArray();
            }
        }

        static bool allUnlocked = false;

        public static void UnlockAllObjects()
        {
            if (allUnlocked)
            {
                return;
            }
            MemoryAccess.UnlockAllObjects();
        }
    }
}
