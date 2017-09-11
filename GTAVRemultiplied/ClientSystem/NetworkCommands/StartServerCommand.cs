using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreneticScript;
using FreneticScript.CommandSystem;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using GTAVRemultiplied.ServerSystem;

namespace GTAVRemultiplied.ClientSystem.NetworkCommands
{
    public class StartServerCommand : AbstractCommand
    {
        // TODO: Meta!

        public StartServerCommand()
        {
            Name = "startserver";
            Arguments = "[port]";
            Description = "Starts hosting a server locally.";
            MinimumArguments = 0;
            MaximumArguments = 1;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                IntegerTag.TryFor
            };
        }
        
        public static void Execute(CommandQueue queue, CommandEntry entry)
        {
            if (GTAVFreneticServer.HostAccount == null)
            {
                queue.HandleError(entry, "Cannot host: not logged in!");
                return;
            }
            ushort port = 28010;
            if (entry.Arguments.Count > 0)
            {
                port = (ushort)IntegerTag.TryFor(entry.GetArgumentObject(queue, 0)).Internal;
            }
            if (GTAVFreneticServer.Enabled)
            {
                queue.HandleError(entry, "Already running a server!");
                return;
            }
            GTAVFreneticServer.Init(port);
            if (entry.ShouldShowGood(queue))
            {
                entry.Good(queue, "Server starting!");
            }
        }
    }
}
