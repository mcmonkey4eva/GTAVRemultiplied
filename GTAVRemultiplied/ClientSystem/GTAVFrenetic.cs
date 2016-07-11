using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreneticScript;
using FreneticScript.CommandSystem;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using System.IO;
using GTAVRemultiplied.ClientSystem.CommonCommands;
using GTAVRemultiplied.ClientSystem.NetworkCommands;
using GTAVRemultiplied.ClientSystem.TagBases;
using GTAVRemultiplied.SharedSystems;

namespace GTAVRemultiplied.ClientSystem
{
    public class GTAVFrenetic
    {
        public static GTAVFreneticOutputter Output;

        public static Commands CommandSystem;

        public static Scheduler Schedule = new Scheduler();

        public static void AutorunScripts()
        {
            string[] files = Directory.GetFiles(Environment.CurrentDirectory + "/frenetic/client/scripts/", "*.cfg", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string cmd = File.ReadAllText(file).Replace("\r", "").Replace("\0", "\\0");
                CommandSystem.PrecalcScript(file.Replace(Environment.CurrentDirectory, ""), cmd);
            }
            CommandSystem.RunPrecalculated();
        }

        public static float cDelta = 0;

        public static double GlobalTickTime = 100;

        public static void Tick(float delta)
        {
            cDelta = delta;
            GlobalTickTime += cDelta;
            Schedule.RunAllSyncTasks(delta);
            CommandSystem.Tick(delta);
        }

        public static void Init()
        {
            Output = new GTAVFreneticOutputter();
            CommandSystem = new Commands();
            Output.Syst = CommandSystem;
            CommandSystem.Output = Output;
            CommandSystem.Init();
            // Common Commands
            CommandSystem.RegisterCommand(new DebugPositionCommand());
            CommandSystem.RegisterCommand(new DevelCommand());
            // Network Commands
            CommandSystem.RegisterCommand(new ConnectCommand());
            CommandSystem.RegisterCommand(new LoginCommand());
            CommandSystem.RegisterCommand(new StartServerCommand());
            CommandSystem.RegisterCommand(new StopServerCommand());
            // Tag Bases
            CommandSystem.TagSystem.Register(new ControlTagBase());
            // Wrap up
            CommandSystem.PostInit();
            AutorunScripts();
        }
    }
}
