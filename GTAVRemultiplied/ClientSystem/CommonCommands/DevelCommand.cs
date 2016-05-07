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

namespace GTAVRemultiplied.ClientSystem.CommonCommands
{
    class DevelCommand : AbstractCommand
    {
        // TODO: Meta!

        public DevelCommand()
        {
            Name = "devel";
            Arguments = "<command>";
            Description = "Runs an experimental/developmental command.";
            MinimumArguments = 1;
            MaximumArguments = 2;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TextTag.For
            };
        }

        public override void Execute(CommandQueue queue, CommandEntry entry)
        {
            TemplateObject arg2 = (entry.Arguments.Count > 1) ? entry.GetArgumentObject(queue, 1) : null;
            switch (entry.GetArgument(queue, 0))
            {
                case "fixPos":
                    ClientConnectionScript.firsttele = false;
                    break;
                case "switchCharacter":
                    PedHash mod;
                    if (Enum.TryParse(arg2.ToString(), true, out mod))
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
                    queue.HandleError(entry, "What?");
                    return;
            }
        }
    }
}
