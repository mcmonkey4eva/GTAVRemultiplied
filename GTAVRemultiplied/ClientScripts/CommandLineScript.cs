using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.UI;
using GTA.Native;
using GTA.Math;
using System.Drawing;
using System.Windows.Forms;
using GTAVRemultiplied;
using GTAVRemultiplied.ServerSystem;
using GTAVRemultiplied.ClientSystem;
using System.Threading;
using System.Globalization;
using System.Runtime.InteropServices;

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
        KeyDown += CommandLineScript_KeyDown;
        KeyUp += CommandLineScript_KeyUp;
        PointF cent = new PointF(GTA.UI.Screen.Width * 0.5f - 150f, GTA.UI.Screen.Height * 0.5f);
        PointF bcent = new PointF(GTA.UI.Screen.Width * 0.5f, GTA.UI.Screen.Height * 0.5f + 30f);
        WritingBack = new ContainerElement(bcent, new SizeF(300f, 60f), Color.Black, true);
        Writing = new TextElement("", cent, 0.25f, Color.White, GTA.UI.Font.ChaletLondon, Alignment.Left, true, false, 300f);
    }

    public bool Shift = false;

    private void CommandLineScript_KeyUp(object sender, KeyEventArgs e)
    {
        if (!OSKVisible || e.Control)
        {
            return;
        }
        if (e.KeyCode == Keys.Shift)
        {
            Shift = false;
        }
        e.SuppressKeyPress = true;
    }

    public ContainerElement WritingBack;

    public TextElement Writing;

    [DllImport("User32.dll", CharSet = CharSet.Unicode)]
    public static extern int ToUnicode(uint virtualKey, uint scanCode, byte[] keyStates, [MarshalAs(UnmanagedType.LPArray)] [Out] char[] chars, int charMaxCount, uint flags);

    public string KB_Entry = "";

    String GetCharsFromKeys(Keys keys, bool shift, bool alt)
    {
        char[] buf = new char[256];
        byte[] kbstate = new byte[256];

        if (shift)
        {
            kbstate[(int)Keys.ShiftKey] = 0xff;
        }

        if (alt)
        {
            kbstate[(int)Keys.ControlKey] = 0xff;
            kbstate[(int)Keys.Menu] = 0xff;
        }

        unsafe
        {
            if (ToUnicode((uint)keys, 0, kbstate, buf, 256, 0) == 1)
            {
                fixed (char* inp = buf)
                {
                    string res = new string(inp);
                    KB_Entry += res;
                }
            }
        }

        return null;
    }

    private void CommandLineScript_KeyDown(object sender, KeyEventArgs e)
    {
        if (!OSKVisible || e.Control)
        {
            return;
        }
        else if (e.KeyCode == Keys.Back)
        {
            if (KB_Entry.Length > 0)
            {
                KB_Entry = KB_Entry.Substring(0, KB_Entry.Length - 1);
            }
        }
        else if (e.KeyCode == Keys.Escape)
        {
            OSKVisible = false;
        }
        else if (e.KeyCode == Keys.Enter)
        {
            OSKVisible = false;
            RunCommand(KB_Entry);
        }
        else if (e.KeyCode == Keys.Shift || e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.RShiftKey || e.KeyCode == Keys.LShiftKey)
        {
            Shift = true;
        }
        else
        {
            string t = GetCharsFromKeys(e.KeyCode, e.Shift, e.Alt);
            KB_Entry += t;
        }
        e.SuppressKeyPress = true;
    }

    KeysConverter ConvKeys = new KeysConverter();

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
        if (OSKVisible)
        {
            return;
        }
        OSKVisible = true;
        KB_Entry = "";
        //Function.Call(Hash.DISPLAY_ONSCREEN_KEYBOARD, 6, "FMMC_KEY_TIP8", "", "", "", "", "", 1024);
    }
    
    private void CommandLineScript_Tick(object sender, EventArgs e)
    {
        try
        {
            if (OSKVisible)
            {
                Game.DisableAllControlsThisFrame();
                WritingBack.Draw();
                Writing.Caption = "] " + KB_Entry;
                Writing.Draw();
                /*int id = Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD);
                if (id == 2)
                {
                    OSKVisible = false;
                }
                else if (id == 1)
                {
                    OSKVisible = false;
                    string res = Function.Call<string>(Hash.GET_ONSCREEN_KEYBOARD_RESULT);
                    RunCommand(res);
                }*/
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
