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
    Text PX;
    Text PY;
    Text PZ;
    Text FX;
    Text FY;
    Text FZ;
    Text CX;
    Text CY;
    Text CZ;

    Text NET;

    public DebugPositionScript()
    {
        Tick += DebugPositionScript_Tick;
        PX = new Text("PosX: ", new System.Drawing.Point(0, 0), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, Alignment.Left);
        PY = new Text("PosY: ", new System.Drawing.Point(0, 16), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, Alignment.Left);
        PZ = new Text("PosZ: ", new System.Drawing.Point(0, 32), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, Alignment.Left);
        FX = new Text("ForX: ", new System.Drawing.Point(0, 48), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, Alignment.Left);
        FY = new Text("ForY: ", new System.Drawing.Point(0, 64), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, Alignment.Left);
        FZ = new Text("ForZ: ", new System.Drawing.Point(0, 80), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, Alignment.Left);
        CX = new Text("CamX: ", new System.Drawing.Point(0, 96), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, Alignment.Left);
        CY = new Text("CamY: ", new System.Drawing.Point(0, 112), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, Alignment.Left);
        CZ = new Text("CamZ: ", new System.Drawing.Point(0, 128), 0.5f, System.Drawing.Color.White, Font.ChaletLondon, Alignment.Left);
        NET = new Text("NET", new System.Drawing.Point(0, 150), 0.5f, System.Drawing.Color.Yellow, Font.ChaletLondon, Alignment.Left);
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
