using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreneticScript;
using FreneticScript.CommandSystem;
using FreneticScript.TagHandlers;
using System.IO;
using FreneticScript.TagHandlers.Objects;

namespace GTAVRemultiplied.ServerSystem
{
    public class GTAVFreneticServerOutputter : Outputter
    {
        public Commands Syst;
        
        public override string ReadTextFile(string name)
        {
            return File.ReadAllText(Environment.CurrentDirectory + "/frenetic/server/scripts/" + name.Replace("..", "_")); // TODO: Proper sandbox!
        }

        public override void UnknownCommand(CommandQueue queue, string basecommand, string[] arguments)
        {
            // TODO: ?
        }

        public override void WriteLine(string text)
        {
            Log.Message("Info", text);
        }

        public override byte[] ReadDataFile(string name)
        {
            return File.ReadAllBytes(Environment.CurrentDirectory + "/frenetic/server/script_data/" + name.Replace("..", "_")); // TODO: Proper sandbox!
        }

        public override void WriteDataFile(string name, byte[] data)
        {
            string path = Environment.CurrentDirectory + "/frenetic/server/script_data/" + name.Replace("..", "_"); // TODO: Proper sandbox!
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllBytes(path, data);
        }

        public override void Reload()
        {
            GTAVFreneticServer.AutorunScripts();
        }

        public override void BadOutput(string text)
        {
            Log.Error(text);
        }

        public override void GoodOutput(string text)
        {
            Log.Message("Info", text);
        }

        public override bool ShouldErrorOnInvalidCommand()
        {
            return true;
        }
    }
}
