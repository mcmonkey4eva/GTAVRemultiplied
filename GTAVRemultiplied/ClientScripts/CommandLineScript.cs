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

/// <summary>
/// Shows a command line for players to type into.
/// Opened by the numpad divide key, closed with the enter key.
/// Server command line opened by the number multiply key.
/// </summary>
public class CommandLineScript : Script
{
    // TODO: Adjust coordinates to scale properly across different resolutions?
    UIRectangle BackRect = new UIRectangle(new Point(UI.WIDTH / 2 - 128, UI.HEIGHT / 2), new Size(512, 64), Color.Black);
    UIText Rendered = new UIText("", new Point(UI.WIDTH / 2 - 128, UI.HEIGHT / 2), 0.5f, Color.White, GTA.Font.ChaletLondon, false);

    public CommandLineScript()
    {
        KeyDown += CommandLineScript_KeyDown;
        Tick += CommandLineScript_Tick;
    }

    bool Visible = false;

    bool Mode = false;

    bool WasDownLast = false;

    bool WasDownLast2 = false;

    string cText = "";
    
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
    
    private void CommandLineScript_KeyDown(object sender, KeyEventArgs e)
    {
        if (!Visible)
        {
            return;
        }
        if (e.KeyCode == Keys.Escape)
        {
            Visible = false;
            return;
        }
        if (e.KeyCode == Keys.Enter)
        {
            RunCommand(cText);
            cText = "";
            Visible = false;
            return;
        }
        if (e.KeyCode == Keys.Back)
        {
            if (cText.Length > 0)
            {
                cText = cText.Substring(0, cText.Length - 1);
            }
            return;
        }
        char c = CharacterUtilities.GetCharFrom(e.KeyCode, e.Shift || Console.CapsLock);
        if (c == '\0')
        {
            return;
        }
        cText += c;
    }

    private void CommandLineScript_Tick(object sender, EventArgs e)
    {
        try
        {
            bool p = Game.IsKeyPressed(Keys.Divide);
            if (p && !WasDownLast)
            {
                cText = "";
                Visible = !Visible;
                Mode = false;
            }
            WasDownLast = p;
            p = Game.IsKeyPressed(Keys.Multiply);
            if (p && GTAVFreneticServer.Enabled && !WasDownLast2)
            {
                cText = "";
                Visible = !Visible;
                Mode = true;
            }
            WasDownLast2 = p;
            if (Visible)
            {
                Game.DisableAllControlsThisFrame(2);
                Rendered.Caption = cText;
                BackRect.Draw();
                Rendered.Draw();
            }
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
        }
    }
}
