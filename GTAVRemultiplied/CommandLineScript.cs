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

/// <summary>
/// Shows a command line for players to type into.
/// Opened by the numpad divide key, closed with the enter key.
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

    bool WasDownLast = false;

    string cText = "";

    public void RunCommand(string cmd)
    {
        UI.ShowSubtitle("[Command] " + cmd); // Placeholder!
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
        e.SuppressKeyPress = true;
        if (e.KeyCode == Keys.Enter)
        {
            RunCommand(cText);
            cText = "";
            Visible = false;
            return;
        }
        if (e.KeyCode == Keys.Back)
        {
            if (cText.Length > 1)
            {
                cText = cText.Substring(0, cText.Length - 1);
            }
            return;
        }
        char newC = e.KeyCode.ToString()[0];
        if (e.Shift)
        {
            newC = char.ToUpperInvariant(newC);
        }
        else
        {
            newC = char.ToLowerInvariant(newC);
        }
        cText += newC;
    }

    private void CommandLineScript_Tick(object sender, EventArgs e)
    {
        bool p = Game.IsKeyPressed(Keys.Divide);
        if (p && !WasDownLast)
        {
            cText = "";
            Visible = !Visible;
        }
        WasDownLast = p;
        if (Visible)
        {
            Game.DisableAllControlsThisFrame(2);
            Rendered.Caption = cText;
            BackRect.Draw();
            Rendered.Draw();
        }
    }
}
