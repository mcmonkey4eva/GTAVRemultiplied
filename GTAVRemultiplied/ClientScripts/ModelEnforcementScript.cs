using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Native;
using GTAVRemultiplied;

public class ModelEnforcementScript : Script
{
    public static Model? WantedModel = null;

    public ModelEnforcementScript()
    {
        Tick += ModelEnforcementScript_Tick;
    }

    static bool CanTick = true;

    private void ModelEnforcementScript_Tick(object sender, EventArgs e)
    {
        if (!CanTick)
        {
            return;
        }
        try
        {
            if (Game.Player.IsDead && WantedModel.HasValue && Game.Player.Character.Model == WantedModel.Value)
            {
                SetModel(PedHash.Michael);
                return;
            }
            if (!Game.Player.IsDead && WantedModel.HasValue && WantedModel.Value != Game.Player.Character.Model)
            {
                if (!SetModel(WantedModel.Value))
                {
                    WantedModel = Game.Player.Character.Model;
                }
            }
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
        }
    }

    bool SetModel(Model mod)
    {
        if (!Function.Call<bool>(Hash.IS_MODEL_IN_CDIMAGE, mod.Hash) || !Function.Call<bool>(Hash.IS_MODEL_VALID, mod.Hash))
        {
            return false;
        }
        CanTick = false;
        Function.Call(Hash.REQUEST_MODEL, mod.Hash);
        while (!Function.Call<bool>(Hash.HAS_MODEL_LOADED, mod.Hash))
        {
            Wait(0);
        }
        bool[] hads = new bool[hashes.Length];
        for (int i = 0; i < hashes.Length; i++)
        {
            hads[i] = Game.Player.Character.Weapons.HasWeapon(hashes[i]);
            // TODO: Get Ammo count for each?
        }
        WeaponHash current = Game.Player.Character.Weapons.Current.Hash;
        Function.Call(Hash.SET_PLAYER_MODEL, Game.Player.Handle, mod.Hash);
        Game.Player.Character.SetDefaultClothes();
        for (int i = 0; i < hashes.Length; i++)
        {
            if (hads[i])
            {
                Game.Player.Character.Weapons.Give(hashes[i], 999, false, true);
            }
        }
        Game.Player.Character.Weapons.Select(current, true);
        CanTick = true;
        Wait(100);
        Function.Call(Hash.SET_MODEL_AS_NO_LONGER_NEEDED, mod.Hash);
        return true;
    }

    public static bool SetClothing(Ped character, int component, int drawable, int texture)
    {
        if (Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, character.Handle, component, drawable, texture))
        {
            Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character.Handle, component, drawable, texture, 0);
            return true;
        }
        return false;
    }

    public static WeaponHash[] hashes = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
}
