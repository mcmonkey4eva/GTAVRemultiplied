using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAVRemultiplied
{
    public class GTAVUtilities
    {
        public static void SwitchCharacter(Model? mod)
        {
            ModelEnforcementScript.WantedModel = mod;
        }
    }
}
