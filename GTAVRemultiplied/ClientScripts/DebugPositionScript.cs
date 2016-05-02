using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Native;
using GTA.Math;
using GTAVRemultiplied;

/// <summary>
/// Shows debug information indicating a player's world position on-screen.
/// Toggled by the numpad multiply key.
/// </summary>
public class DebugPositionScript : Script
{
    UIText PX;
    UIText PY;
    UIText PZ;
    UIText FX;
    UIText FY;
    UIText FZ;
    UIText CX;
    UIText CY;
    UIText CZ;

    public DebugPositionScript()
    {
        Tick += DebugPositionScript_Tick;
        // TODO: Test these positions on different resolutions and adjust values accordingly? (Perhaps they should be a percentage of screen height?)
        PX = new UIText("PosX: ", new System.Drawing.Point(0, UI.HEIGHT / 4), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, false);
        PY = new UIText("PosY: ", new System.Drawing.Point(0, UI.HEIGHT / 4 + 16), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, false);
        PZ = new UIText("PosZ: ", new System.Drawing.Point(0, UI.HEIGHT / 4 + 32), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, false);
        FX = new UIText("ForX: ", new System.Drawing.Point(0, UI.HEIGHT / 4 + 48), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, false);
        FY = new UIText("ForY: ", new System.Drawing.Point(0, UI.HEIGHT / 4 + 64), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, false);
        FZ = new UIText("ForZ: ", new System.Drawing.Point(0, UI.HEIGHT / 4 + 80), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, false);
        CX = new UIText("CamX: ", new System.Drawing.Point(0, UI.HEIGHT / 4 + 96), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, false);
        CY = new UIText("CamY: ", new System.Drawing.Point(0, UI.HEIGHT / 4 + 112), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, false);
        CZ = new UIText("CamZ: ", new System.Drawing.Point(0, UI.HEIGHT / 4 + 128), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, false);
    }

    bool Enabled = false;

    bool WasDownLast = false;

    private void DebugPositionScript_Tick(object sender, EventArgs e)
    {
        try
        {
            bool p = Game.IsKeyPressed(System.Windows.Forms.Keys.Multiply);
            if (p && !WasDownLast)
            {
                Enabled = !Enabled;
            }
            WasDownLast = p;
            if (Enabled)
            {
                PX.Caption = "PosX: " + Game.Player.Character.Position.X;
                PY.Caption = "PosY: " + Game.Player.Character.Position.Y;
                PZ.Caption = "PosZ: " + Game.Player.Character.Position.Z;
                FX.Caption = "ForX: " + Game.Player.Character.ForwardVector.X;
                FY.Caption = "ForY: " + Game.Player.Character.ForwardVector.Y;
                FZ.Caption = "ForZ: " + Game.Player.Character.ForwardVector.Z;
                CX.Caption = "CamX: " + GameplayCamera.Direction.X;
                CY.Caption = "CamY: " + GameplayCamera.Direction.Y;
                CZ.Caption = "CamZ: " + GameplayCamera.Direction.Z;
                PX.Draw();
                PY.Draw();
                PZ.Draw();
                FX.Draw();
                FY.Draw();
                FZ.Draw();
                CX.Draw();
                CY.Draw();
                CZ.Draw();
            }
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
        }
    }
}
