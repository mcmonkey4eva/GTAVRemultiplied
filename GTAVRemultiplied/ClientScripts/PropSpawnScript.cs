using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using FreneticScript;
using GTAVRemultiplied;

public class PropSpawnScript : Script
{
    public static ListQueue<Tuple<int, Vector3, int>> propsToSpawn = new ListQueue<Tuple<int, Vector3, int>>();

    public PropSpawnScript()
    {
        Tick += PropSpawnScript_Tick;
    }

    private void PropSpawnScript_Tick(object sender, EventArgs e)
    {
        while (propsToSpawn.Length > 0)
        {
            Prop prop = World.CreateProp(new Model(propsToSpawn[0].Item1), propsToSpawn[0].Item2, false, false);
            if (prop == null)
            {
                Log.Message("Warning", "invalid prop: " + propsToSpawn[0].Item1, 'Y');
            }
            else
            {
                prop.IsPersistent = true;
                prop.IsInvincible = true;
                prop.FreezePosition = true;
                ClientConnectionScript.ServerToClientProp[propsToSpawn[0].Item3] = prop.Handle;
                ClientConnectionScript.ClientToServerProp[prop.Handle] = propsToSpawn[0].Item3;
            }
            propsToSpawn.Pop();
        }
    }
}
