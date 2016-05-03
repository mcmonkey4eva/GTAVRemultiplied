using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
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
    }
}
