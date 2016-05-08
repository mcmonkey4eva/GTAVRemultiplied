using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;
using FreneticScript;

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

        static string[] all_ipls = new string[]
        {
            "airfield", "AP1_04_TriAf01", "bh1_16_refurb", "BH1_47_JoshHse_Burnt", "bh1_47_joshhse_firevfx", "BH1_47_JoshHse_UnBurnt", "BH1_48_Killed_Michael", "bnkheist_apt_dest", "bnkheist_apt_norm",
            "burgershot_yoga", "burnt_switch_off", "cargoship", "canyonriver01", "railing_start", "canyonriver01_traincrash", "railing_end", "chemgrill_grp1", "CH1_07_TriAf03", "CH3_RD2_BishopsChickenGraffiti",
            "chop_props", "chophillskennel", "CJ_IOABoat", "coronertrash", "Coroner_Int_off", "Coroner_Int_on", "crashed_cargoplane", "CS1_02_cf_offmission", "CS1_02_cf_onmission1", "CS1_02_cf_onmission2",
            "CS1_02_cf_onmission3", "CS1_02_cf_onmission4", "CS2_06_TriAf02", "CS3_05_water_grp1", "CS3_05_water_grp2", "cs3_07_mpgates", "CS4_08_TriAf02", "CS4_04_TriAf03", "CS5_04_MazeBillboardGraffiti",
            "cs5_4_trains", "CS5_Roads_RonOilGraffiti", "des_farmhouse", "des_farmhs_endimap", "des_farmhs_end_occl", "des_farmhs_startimap", "des_farmhs_start_occl", "DES_ProTree_start", "DES_ProTree_start_lod",
            "DES_Smash2_endimap", "DES_Smash2_startimap", "DES_StiltHouse_imapend", "DES_StiltHouse_imapstart", "des_stilthouse_rebuild", "DT1_05_HC_REMOVE", "DT1_05_HC_REQ", "DT1_05_REQUEST", "DT1_05_rubble",
            "DT1_17_OldBill", "DT1_17_NewBill", "DT1_21_prop_lift_on", "fakeint", "farm", "farm_burnt", "farm_burnt_props", "farmint_cap", "farmint", "farm_props", "FBI_colPLUG", "FIBlobby", "FIBlobbyfake",
            "FBI_repair", "ferris_finale_Anim", "FruitBB", "gasparticle_grp2", "gasstation_ipl_group1", "gasstation_ipl_group2", "hei_carrier", "hei_carrier_DistantLights", "hei_Carrier_int1", "hei_Carrier_int2",
            "hei_Carrier_int3", "hei_Carrier_int4", "hei_Carrier_int5", "hei_Carrier_int6", "hei_carrier_LODLights", "hei_yacht_heist", "hei_yacht_heist_Bar", "hei_yacht_heist_Bedrm", "hei_yacht_heist_Bridge",
            "hei_yacht_heist_DistantLights", "hei_yacht_heist_enginrm", "hei_yacht_heist_LODLights", "hei_yacht_heist_Lounge", "id2_14_during_door", "id2_14_during1", "id2_14_during2", "id2_14_on_fire",
            "id2_14_post_no_int", "id2_14_pre_no_int", "ID2_21_G_Night", "Jetsteal_ipl_grp1", "Jetsteal_ipl_grp2", "jetstealtunnel", "jewel2fake", "Jewel_Gasmasks", "layer_sextoys_a", "layer_torture",
            "ld_rail_02_track", "MG-Flight School 5", "Michael_premier", "occl_meth_grp1", "Plane_crash_trench", "post_hiest_unload", "prologue01", "prologue01c", "prologue01d", "prologue01e", "prologue01f",
            "prologue01g", "prologue01h", "prologue01i", "prologue01j", "prologue01k", "prologue01z", "prologue02", "prologue03", "prologue03b", "prologue03_grv_fun", "prologue04", "prologue04b", "prologue05",
            "prologue05b", "prologue06", "prologue06b", "prologue06_int", "prologuerd", "prologuerdb", "prologue_DistantLights", "prologue_LODLights", "prologue_m2_door", "facelobby", "prop_cheetah_covered",
            "prop_entityXF_covered", "prop_jb700_covered", "prop_ztype_covered", "RC12B_Default", "RC12B_Destroyed", "RC12B_Fixed", "RC12B_HospitalInterior", "refit_unload", "REMOVE_ALL_STATES", "SC1_01_NewBill",
            "SC1_01_OldBill", "SC1_30_Keep_Closed", "ship_occ_grp1", "ship_occ_grp2", "shr_int", "smboat", "SM_15_BldGRAF1", "sunkcargoship", "tankerexp_grp0", "tankerexp_grp1", "tankerexp_grp2", "tankerexp_grp3",
            "TrevorsMP", "TrevorsTrailer", "TrevorsTrailerTidy", "TrevorsTrailerTrash", "triathlon2_VBprops", "TRV1_Trail_end", "TRV1_Trail_Finish", "TRV1_Trail_start", "UFO", "V_35_Fireman", "VB_08_TriAf01",
            "v_carshowroom", "shutter_open", "shutter_closed", "shr_int", "csr_inMission", "fakeint", "V_Michael", "V_Michael_Garage", "V_Michael_FameShame", "V_Michael_JewelHeist", "V_Michael_plane_ticket",
            "V_Michael_Scuba", "v_tunnel_hole", "v_tunnel_hole_swap", "yogagame", "FINBANK", "DT1_03_Shutter", "DT1_03_Gr_Closed", "DES_tankercrash"
        };

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
            byte[] data = new byte[all_ipls.Length];
            int i = 0;
            foreach (string str in all_ipls)
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
            if (data.Length != all_ipls.Length)
            {
                throw new Exception("Invalid IPL data!");
            }
            int i = 0;
            foreach (string str in all_ipls)
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
    }
}
