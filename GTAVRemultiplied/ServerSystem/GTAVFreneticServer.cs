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
using GTAVRemultiplied.ServerSystem.CommonCommands;
using GTAVRemultiplied.ServerSystem.WorldCommands;
using GTAVRemultiplied.ServerSystem.TagBases;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVRemultiplied.ServerSystem
{
    public class GTAVFreneticServer
    {
        public static bool Enabled = false;

        public static GTAVFreneticServerOutputter Output;

        public static Commands CommandSystem;

        public static void AutorunScripts()
        {
            string[] files = Directory.GetFiles(Environment.CurrentDirectory + "/frenetic/server/scripts/", "*.cfg", SearchOption.AllDirectories);
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
            if (Enabled)
            {
                CommandSystem.Tick(delta);
            }
        }

        public static void Shutdown()
        {
            if (!Enabled)
            {
                return;
            }
            Enabled = false;
            Connections.Listener.Stop();
        }

        public static ushort CurrentPort = 28010;

        public static GTAVServerConnection Connections;

        public static void Init(ushort port)
        {
            if (Enabled)
            {
                return;
            }
            Enabled = true;
            if (!ModelEnforcementScript.WantedModel.HasValue)
            {
                GTAVUtilities.SwitchCharacter(PedHash.DeadHooker);
            }
            Connections = new GTAVServerConnection();
            Connections.Listen(port);
            Output = new GTAVFreneticServerOutputter();
            CommandSystem = new Commands();
            Output.Syst = CommandSystem;
            CommandSystem.Output = Output;
            CommandSystem.Init();
            // Common Commands
            CommandSystem.RegisterCommand(new DevelCommand());
            // World Commands
            CommandSystem.RegisterCommand(new ModVehicleCommand());
            // Tag Bases
            CommandSystem.TagSystem.Register(new GameTagBase());
            // TODO: Register tag types!
            // Wrap up
            CommandSystem.PostInit();
            AutorunScripts();
        }
    }
}
