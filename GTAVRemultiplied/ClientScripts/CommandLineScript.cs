using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Native;
using GTA.Math;
using System.Drawing;
using System.Windows.Forms;
using GTAVRemultiplied;
using GTAVRemultiplied.ServerSystem;
using GTAVRemultiplied.ClientSystem;
using System.Threading;
using System.Globalization;

/// <summary>
/// Shows a command line for players to type into.
/// Opened by the numpad divide key, closed with the enter key.
/// Server command line opened by the number multiply key.
/// </summary>
public class CommandLineScript : Script
{
    public CommandLineScript()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        Tick += CommandLineScript_Tick;
    }
    
    bool Mode = false;

    bool WasDownLast = false;

    bool WasDownLast2 = false;
    
    bool OSKVisible = false;
    
    public void RunCommand(string cmd)
    {
        if (Mode)
        {
            if (GTAVFreneticServer.Enabled)
            {
                Log.Message("Server Command", cmd, 'G');
                GTAVFreneticServer.CommandSystem.ExecuteCommands(cmd, null);
            }
        }
        else
        {
            Log.Message("Client Command", cmd, 'G');
            GTAVFrenetic.CommandSystem.ExecuteCommands(cmd, null);
        }
    }

    private void OSK()
    {
        OSKVisible = true;
        Function.Call(Hash.DISPLAY_ONSCREEN_KEYBOARD, 6, "FMMC_KEY_TIP8", "", "", "", "", "", 1024);
    }
    
    private void CommandLineScript_Tick(object sender, EventArgs e)
    {
        try
        {
            if (OSKVisible)
            {
                int id = Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD);
                if (id == 2)
                {
                    OSKVisible = false;
                }
                else if (id == 1)
                {
                    OSKVisible = false;
                    string res = Function.Call<string>(Hash.GET_ONSCREEN_KEYBOARD_RESULT);
                    RunCommand(res);
                }
                return;
            }
            bool p = Game.IsKeyPressed(Keys.Divide);
            if (p && !WasDownLast)
            {
                OSK();
                Mode = false;
            }
            WasDownLast = p;
            p = Game.IsKeyPressed(Keys.Multiply);
            if (p && GTAVFreneticServer.Enabled && !WasDownLast2)
            {
                OSK();
                Mode = true;
            }
            WasDownLast2 = p;
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
        }
    }
}
