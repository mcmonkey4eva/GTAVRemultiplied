using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Native;

public class ModelEnforcementScript : Script
{
    public static Model? WantedModel = null;

    public ModelEnforcementScript()
    {
        Tick += ModelEnforcementScript_Tick;
    }

    bool CanTick = true;

    private void ModelEnforcementScript_Tick(object sender, EventArgs e)
    {
        if (!CanTick)
        {
            return;
        }
        if (WantedModel != null && WantedModel.HasValue && WantedModel.Value != Game.Player.Character.Model)
        {
            if (!Function.Call<bool>(Hash.IS_MODEL_IN_CDIMAGE, WantedModel.Value.Hash) || !Function.Call<bool>(Hash.IS_MODEL_VALID, WantedModel.Value.Hash))
            {
                WantedModel = null;
                return;
            }
            CanTick = false;
            Function.Call(Hash.REQUEST_MODEL, WantedModel.Value.Hash);
            while (!Function.Call<bool>(Hash.HAS_MODEL_LOADED, WantedModel.Value.Hash))
            {
                Wait(0);
            }
            Function.Call(Hash.SET_PLAYER_MODEL, Game.Player.Handle, WantedModel.Value.Hash);
            Game.Player.Character.SetDefaultClothes();
            CanTick = true;
        }
    }
}
