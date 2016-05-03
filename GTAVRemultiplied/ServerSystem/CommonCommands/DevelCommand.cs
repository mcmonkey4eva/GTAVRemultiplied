using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreneticScript;
using FreneticScript.CommandSystem;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using GTA;
using GTA.Native;

namespace GTAVRemultiplied.ServerSystem.CommonCommands
{
    public class DevelCommand : AbstractCommand
    {
        public DevelCommand()
        {
            Name = "devel";
            Arguments = "<command> [argument]";
            Description = "Runs an experimental serverside command.";
            MinimumArguments = 1;
            MaximumArguments = 2;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TextTag.For,
                TextTag.For
            };
        }

        public override void Execute(CommandQueue queue, CommandEntry entry)
        {
            string cmd = entry.GetArgument(queue, 0);
            switch (cmd)
            {
                case "switchCharacter":
                    PedHash mod;
                    if (entry.Arguments.Count > 1 && Enum.TryParse(entry.GetArgument(queue, 1), true, out mod))
                    {
                        GTAVUtilities.SwitchCharacter(mod);
                        if (entry.ShouldShowGood(queue))
                        {
                            entry.Good(queue, "Character switched!");
                        }
                    }
                    else
                    {
                        queue.HandleError(entry, "Invalid input!");
                    }
                    break;
                default:
                    queue.HandleError(entry, "Invalid devel command!");
                    break;
            }
        }
    }
}
