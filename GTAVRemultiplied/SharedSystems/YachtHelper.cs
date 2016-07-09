using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVRemultiplied.SharedSystems
{
    class YachtHelper
    {
        public static KeyValuePair<Vector3, float>[] YachtPositions = new KeyValuePair<Vector3, float>[]
        {
            new KeyValuePair<Vector3, float>(new Vector3(-3148.379f, 2807.555f, -4.620046f), 91.95499f),
            new KeyValuePair<Vector3, float>(new Vector3(-3254.552f, 3685.677f, -4.620046f), 81.95504f),
            new KeyValuePair<Vector3, float>(new Vector3(-3448.254f, 311.5018f, -4.620046f), -83.0449f),
            new KeyValuePair<Vector3, float>(new Vector3(-2117.581f, -2543.346f, -4.620046f), 36.95498f),
            new KeyValuePair<Vector3, float>(new Vector3(-351.0608f, -3553.323f, -4.620046f), -123.0449f),
            new KeyValuePair<Vector3, float>(new Vector3(2490.885f, -2428.848f, -4.620046f), -168.045f),
            new KeyValuePair<Vector3, float>(new Vector3(3021.254f, -723.3903f, -4.620046f), 81.95504f),
            new KeyValuePair<Vector3, float>(new Vector3(3411.1f, 1193.445f, -4.620046f), 31.95502f),
            new KeyValuePair<Vector3, float>(new Vector3(4250.581f, 4576.565f, -4.620046f), 111.955f),
            new KeyValuePair<Vector3, float>(new Vector3(3490.105f, 6305.785f, -4.620046f), 156.955f),
            new KeyValuePair<Vector3, float>(new Vector3(2004.462f, 6907.157f, -4.620046f), 6.955003f),
            new KeyValuePair<Vector3, float>(new Vector3(-777.4865f, 6566.907f, -4.620046f), 26.95494f)
        };

        public static KeyValuePair<Vector3, float> GetYachtPos(int id)
        {
            return YachtPositions[id - 1];
        }

        public static Dictionary<int, KeyValuePair<int, List<Prop>>> spawnedYachts = new Dictionary<int, KeyValuePair<int, List<Prop>>>();

        public static void SpawnYacht(int id, int opt, bool goldRails, string lights)
        {
            KeyValuePair<int, List<Prop>> spawned;
            if (spawnedYachts.TryGetValue(id, out spawned))
            {
                Function.Call(Hash.REMOVE_IPL, "apa_yacht_grp" + id.ToString("00") + "_" + spawned.Key);
                Function.Call(Hash.REMOVE_IPL, "apa_yacht_grp" + id.ToString("00") + "_" + spawned.Key + "_int");
                Function.Call(Hash.REMOVE_IPL, "apa_yacht_grp" + id.ToString("00") + "_" + spawned.Key + "_lod");
                foreach (Prop prop in spawned.Value)
                {
                    if (prop != null)
                    {
                        prop.Delete();
                    }
                }
                spawnedYachts.Remove(id);
            }
            int topt = 2; // TODO: Handle alternate positions?
            Function.Call(Hash.REQUEST_IPL, "apa_yacht_grp" + id.ToString("00") + "_" + topt);
            Function.Call(Hash.REQUEST_IPL, "apa_yacht_grp" + id.ToString("00") + "_" + topt + "_int");
            Function.Call(Hash.REQUEST_IPL, "apa_yacht_grp" + id.ToString("00") + "_" + topt + "_lod");
            Vector3 position = GetYachtPos(id).Key;
            float rotation = GetYachtPos(id).Value;
            Vector3 v1;
            Vector3 v2;
            Vector3 v3;
            Vector3 v4;
            switch (opt)
            {
                case 1:
                    v1 = new Vector3(0f, 0f, 9.1f);
                    v2 = new Vector3(0f, 0f, 4.65f);
                    v3 = new Vector3(0f, 0f, 0f);
                    v4 = new Vector3(0f, 0f, 0f);
                    break;
                case 2:
                    v1 = new Vector3(0f, 0f, 9.5f);
                    v2 = new Vector3(0f, 0f, 9.1f);
                    v3 = new Vector3(0f, 0f, 4.65f);
                    v4 = new Vector3(0f, 0f, 0f);
                    break;
                default:
                    v1 = new Vector3(0f, 0f, 4.8f);
                    v2 = new Vector3(0f, 0f, 4.65f);
                    v3 = new Vector3(0f, 0f, 4.65f);
                    v4 = new Vector3(0f, 0f, 4.65f);
                    break;
            }
            List<Prop> props = new List<Prop>();
            string rail = (goldRails ? "a" : "b");
            // TODO: Create all the props in a dedicated script?! (CreateProp will Wait() on the current script - freezing the command engine!)
            props.Add(World.CreateProp(new Model(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_apa_yacht_o" + opt + "_rail_" + rail)), position + new Vector3(0f, 0f, 2.8f), new Vector3(0f, 0f, rotation), false, false));
            props.Add(World.CreateProp(new Model(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_apa_yacht_option" + opt)), position + v1, new Vector3(0f, 0f, rotation), false, false));
            props.Add(World.CreateProp(new Model(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_apa_yacht_option" + opt + "_cola")), position + v2, new Vector3(0f, 0f, rotation), false, false));
            if (opt >= 2)
            {
                props.Add(World.CreateProp(new Model(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_apa_yacht_option" + opt + "_colb")), position + v3, new Vector3(0f, 0f, rotation), false, false));
            }
            if (opt == 3)
            {
                props.Add(World.CreateProp(new Model(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_apa_yacht_option" + opt + "_colc")), position + v4, new Vector3(0f, 0f, rotation), false, false));
                props.Add(World.CreateProp(new Model(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_apa_yacht_option" + opt + "_cold")), position + v4, new Vector3(0f, 0f, rotation), false, false));
                props.Add(World.CreateProp(new Model(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_apa_yacht_option" + opt + "_cole")), position + v4, new Vector3(0f, 0f, rotation), false, false));
            }
            props.Add(World.CreateProp(new Model(Function.Call<int>(Hash.GET_HASH_KEY, "apa_mp_apa_y "+ opt + "_l" + lights)), position + new Vector3(0f, 0f, 5.9f), new Vector3(0f, 0f, rotation), false, false));
            // TODO: Other props (EG, doors?)
            foreach (Prop prop in props)
            {
                if (prop != null)
                {
                    prop.IsPositionFrozen = true;
                    prop.IsPersistent = true;
                }
                // TODO: else, enforce prop better?
            }
            spawnedYachts[id] = new KeyValuePair<int, List<Prop>>(topt, props);
        }
    }
}
