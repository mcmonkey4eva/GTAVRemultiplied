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

        public override void Bad(string tagged_text, DebugMode mode)
        {
            Log.Error(Syst.TagSystem.ParseTagsFromText(tagged_text, "", new Dictionary<string, TemplateObject>(), mode, (o) => { throw new Exception(o); }, true));
        }

        public override void Good(string tagged_text, DebugMode mode)
        {
            Log.Message("Info", Syst.TagSystem.ParseTagsFromText(tagged_text, "", new Dictionary<string, TemplateObject>(), mode, (o) => { throw new Exception(o); }, true));
        }

        public override string ReadTextFile(string name)
        {
            return File.ReadAllText(Environment.CurrentDirectory + "/frenetic/server/scripts/" + name.Replace("..", "_")); // TODO: Proper sandbox!
        }

        public override void UnknownCommand(CommandQueue queue, string basecommand, string[] arguments)
        {
            Bad("Invalid command: " + TagParser.Escape(basecommand) + "!", queue.CommandStack.Count > 0 ? queue.CommandStack.Peek().Debug : DebugMode.FULL);
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
    }
}
