using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVRemultiplied.ServerSystem
{
    public class PedInfo
    {
        public bool pjump = false;
        public bool wasInVehicle = false;
        public int ammo = 0;
        public WeaponHash weap = WeaponHash.Unarmed;
        public DateTime nextVehicleReminder = DateTime.Now;
        public bool ForcePersistent = false;
    }
}
