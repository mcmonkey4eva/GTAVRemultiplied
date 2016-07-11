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

    const int Count = 15;
    
    const int Height = 12;

    int CPos = (int)GTA.UI.Screen.Height / 3 - Height;

    public Text GenOne()
    {
        CPos += Height;
        return new Text("", new Point(0, CPos), 0.33f, Color.White, GTA.UI.Font.ChaletLondon, Alignment.Left, true, false);
    }

    public List<Text> ChatTextSlot = new List<Text>();

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
        if (AllText.Count > Count * 2)
        {
            AllText.RemoveRange(0, Count);
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
        try
        {
            // TODO: Smooth fade out?
            double dist = DateTime.Now.Subtract(LastUpdate).TotalSeconds;
            if (dist <= 8)
            {
                for (int i = 0; i < ChatTextSlot.Count; i++)
                {
                    Color col = ChatTextSlot[i].Color;
                    ChatTextSlot[i].Color = Color.FromArgb(255, col.R, col.G, col.B);
                    ChatTextSlot[i].Draw();
                }
            }
            else if (dist <= 10)
            {
                byte A = (byte)((1f - (((float)dist - 8f) / 2f)) * 255f);
                for (int i = 0; i < ChatTextSlot.Count; i++)
                {
                    Color col = ChatTextSlot[i].Color;
                    ChatTextSlot[i].Color = Color.FromArgb(A, col.R, col.G, col.B);
                    ChatTextSlot[i].Draw();
                }
            }
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
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
