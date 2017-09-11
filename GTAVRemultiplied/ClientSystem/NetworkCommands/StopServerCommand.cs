using FreneticScript.CommandSystem;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using GTAVRemultiplied.ServerSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ClientSystem.NetworkCommands
{
    public class StopServerCommand : AbstractCommand
    {
        // TODO: Meta!

        public StopServerCommand()
        {
            Name = "stopserver";
            Arguments = "";
            Description = "Stops a locally running server.";
            MinimumArguments = 0;
            MaximumArguments = 0;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>();
        }

        public static void Execute(CommandQueue queue, CommandEntry entry)
        {
            if (!GTAVFreneticServer.Enabled)
            {
                queue.HandleError(entry, "Not running a server!");
                return;
            }
            GTAVFreneticServer.Shutdown();
            if (entry.ShouldShowGood(queue))
            {
                entry.Good(queue, "Server stopping!");
            }
        }
    }
}
