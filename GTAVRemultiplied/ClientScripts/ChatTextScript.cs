﻿using System;
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
using GTAVRemultiplied.ClientSystem;
using GTAVRemultiplied.ServerSystem;
using System.Diagnostics;
using FreneticScript;

/// <summary>
/// Shows a chat/log section along the side of the screen.
/// Also, starts up FreneticScript.
/// </summary>
public class ChatTextScript : Script
{
    public static ChatTextScript Instance;

    const int Count = 7;
    
    const int Height = 12;

    int CPos = UI.HEIGHT / 3 - Height;

    public UIText GenOne()
    {
        CPos += Height;
        return new UIText("", new Point(0, CPos), 0.33f, Color.White, GTA.Font.ChaletLondon, false, true, false);
    }

    public List<UIText> ChatTextSlot = new List<UIText>();

    List<string> AllText = new List<string>();

    DateTime LastUpdate = DateTime.Now;

    public ChatTextScript()
    {
        Instance = this;
        Tick += ChatTextScript_Tick;
        AddLine('c', "GTAV-RMP", "-- Welcome to GTAV Remultiplied! --");
        for (int i = 0; i < Count; i++)
        {
            ChatTextSlot.Add(GenOne());
        }
        TextStyle.Color_Simple = "";
        TextStyle.Reset = "";
        TextStyle.White = "";
        TextStyle.Color_Standout = "";
        TextStyle.Color_Importantinfo = "";
        TextStyle.Color_Commandhelp = "";
        TextStyle.Color_Error = "";
        TextStyle.Color_Outgood = "";
        TextStyle.Color_Outbad = "";
        TextStyle.Color_Separate = "";
        TextStyle.Color_Readable = "";
        TextStyle.Color_Warning = "";
        TextStyle.Default = "";
        GTAVFrenetic.Init();
        sw.Start();
    }

    public void AddLine(char col, string type, string txt)
    {
        AllText.Add(col + "[" + type + "] " + txt);
        if (AllText.Count > 14)
        {
            AllText.RemoveRange(0, 7);
        }
        int start = AllText.Count >= Count ? AllText.Count - Count : 0;
        List<string> substrs = AllText.GetRange(start, AllText.Count - start);
        for (int i = 0; i < ChatTextSlot.Count; i++)
        {
            ChatTextSlot[i].Caption = "";
        }
        for (int i = 0; i < substrs.Count & i < ChatTextSlot.Count; i++)
        {
            char tcol = substrs[i][0];
            switch (tcol)
            {
                case 'R':
                    ChatTextSlot[i].Color = Color.DarkRed;
                    break;
                case 'r':
                    ChatTextSlot[i].Color = Color.Red;
                    break;
                case 'G':
                    ChatTextSlot[i].Color = Color.DarkGreen;
                    break;
                case 'g':
                    ChatTextSlot[i].Color = Color.Green;
                    break;
                case 'c':
                    ChatTextSlot[i].Color = Color.Cyan;
                    break;
                case 'C':
                    ChatTextSlot[i].Color = Color.DarkCyan;
                    break;
                case 'Y':
                    ChatTextSlot[i].Color = Color.FromArgb(128, 128, 0); // DarkYellow.
                    break;
                case 'y':
                    ChatTextSlot[i].Color = Color.Yellow;
                    break;
                default:
                    ChatTextSlot[i].Color = Color.White;
                    break;
            }
            ChatTextSlot[i].Caption = substrs[i].Substring(1);
        }
        LastUpdate = DateTime.Now;
    }

    Stopwatch sw = new Stopwatch();

    private void ChatTextScript_Tick(object sender, EventArgs e)
    {
        // NOTE: If our log renderer errors... just give up and let it crash!
        // TODO: Smooth fade out?
        if (DateTime.Now.Subtract(LastUpdate).TotalSeconds <= 5)
        {
            for (int i = 0; i < ChatTextSlot.Count; i++)
            {
                ChatTextSlot[i].Draw();
            }
        }
        sw.Stop();
        float delt = sw.ElapsedTicks / (float)Stopwatch.Frequency;
        sw.Reset();
        sw.Start();
        try
        {
            GTAVFrenetic.Tick(delt);
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
        }
        try
        {
            GTAVFreneticServer.Tick(delt);
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
        }
    }
}