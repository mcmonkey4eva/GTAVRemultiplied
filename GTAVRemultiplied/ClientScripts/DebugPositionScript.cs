using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Native;
using GTA.UI;
using GTA.Math;
using GTAVRemultiplied;

/// <summary>
/// Shows debug information indicating a player's world position on-screen.
/// Toggled by client command: "debugposition true/false"
/// </summary>
public class DebugPositionScript : Script
{
    TextElement PX;
    TextElement PY;
    TextElement PZ;
    TextElement FX;
    TextElement FY;
    TextElement FZ;
    TextElement CX;
    TextElement CY;
    TextElement CZ;

    TextElement NET;

    public DebugPositionScript()
    {
        Tick += DebugPositionScript_Tick;
        PX = new TextElement("PosX: ", new System.Drawing.Point(0, 0), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, Alignment.Left);
        PY = new TextElement("PosY: ", new System.Drawing.Point(0, 16), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, Alignment.Left);
        PZ = new TextElement("PosZ: ", new System.Drawing.Point(0, 32), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, Alignment.Left);
        FX = new TextElement("ForX: ", new System.Drawing.Point(0, 48), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, Alignment.Left);
        FY = new TextElement("ForY: ", new System.Drawing.Point(0, 64), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, Alignment.Left);
        FZ = new TextElement("ForZ: ", new System.Drawing.Point(0, 80), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, Alignment.Left);
        CX = new TextElement("CamX: ", new System.Drawing.Point(0, 96), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, Alignment.Left);
        CY = new TextElement("CamY: ", new System.Drawing.Point(0, 112), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, Alignment.Left);
        CZ = new TextElement("CamZ: ", new System.Drawing.Point(0, 128), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, Alignment.Left);
        NET = new TextElement("NET", new System.Drawing.Point(0, 150), 0.5f, System.Drawing.Color.Yellow, Font.ChaletLondon, Alignment.Left);
    }

    public static bool Enabled = false;

    private void DebugPositionScript_Tick(object sender, EventArgs e)
    {
        try
        {
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
                NET.Caption = "NET: " + (GTAVRemultiplied.ServerSystem.GTAVFreneticServer.DataUsage / 1024);
                PX.Draw();
                PY.Draw();
                PZ.Draw();
                FX.Draw();
                FY.Draw();
                FZ.Draw();
                CX.Draw();
                CY.Draw();
                CZ.Draw();
                NET.Draw();
            }
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
        }
    }
}
