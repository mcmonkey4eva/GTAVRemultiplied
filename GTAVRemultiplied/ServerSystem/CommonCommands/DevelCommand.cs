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
using GTA.Math;
using GTA.Native;

namespace GTAVRemultiplied.ServerSystem.CommonCommands
{
    public class DevelCommand : AbstractCommand
    {
        public DevelCommand()
        {
            Name = "devel";
            Arguments = "<command> <argument>";
            Description = "Runs an experimental serverside command.";
            MinimumArguments = 2;
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
            TemplateObject arg2 = entry.GetArgumentObject(queue, 1);
            switch (cmd)
            {
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
                case "teleportTo":
                        // TODO: Vector/Location tag!
                        Game.Player.Character.Position = GTAVUtilities.StringToVector(arg2.ToString());
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Teleported!");
                    }
                    break;
                case "teleportSafe":
                    // TODO: Vector/Location tag!
                    Game.Player.Character.Position = GTAVUtilities.StringToVector(arg2.ToString());
                    Game.Player.Character.Position += new Vector3(0, 0, -Game.Player.Character.HeightAboveGround);
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Teleported safely!");
                    }
                    break;
                default:
                    queue.HandleError(entry, "Invalid devel command!");
                    break;
            }
        }
    }
}
