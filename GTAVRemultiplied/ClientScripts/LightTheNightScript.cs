using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;

class LightTheNightScript : Script
{
    public static bool Light = false;

    public LightTheNightScript()
    {
        Tick += LightTheNightScript_Tick;
    }

    private void LightTheNightScript_Tick(object sender, EventArgs e)
    {
        if (Light)
        {
            World.DrawLightWithRange(Game.Player.Character.Position + new Vector3(0, 0, 10), System.Drawing.Color.White, 1000, 1);
            World.DrawLightWithRange(Game.Player.Character.Position, System.Drawing.Color.White, 10, 1);
        }
    }
}
