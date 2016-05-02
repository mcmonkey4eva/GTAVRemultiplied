using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreneticScript;
using FreneticScript.CommandSystem;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;

namespace GTAVRemultiplied.ClientSystem.CommonCommands
{
    class DebugPositionCommand : AbstractCommand
    {
        public DebugPositionCommand()
        {
            Name = "debugposition";
            Arguments = "<boolean>";
            Description = "Toggles whether the position debugger is running.";
            MinimumArguments = 1;
            MaximumArguments = 1;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                BooleanTag.TryFor
            };
        }

        public override void Execute(CommandQueue queue, CommandEntry entry)
        {
            DebugPositionScript.Enabled = BooleanTag.TryFor(entry.GetArgumentObject(queue, 0)).Internal;
            if (entry.ShouldShowGood(queue))
            {
                entry.Good(queue, "Debug Position now: " + (DebugPositionScript.Enabled ? "enabled" : "disabled") + ".");
            }
        }
    }
}
