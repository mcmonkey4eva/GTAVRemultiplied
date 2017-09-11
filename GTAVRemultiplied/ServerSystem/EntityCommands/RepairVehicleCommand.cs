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
using GTAVRemultiplied.ServerSystem.TagObjects;


namespace GTAVRemultiplied.ServerSystem.EntityCommands
{
    public class RepairVehicleCommand : AbstractCommand
    {
        // TODO: Meta!

        public RepairVehicleCommand()
        {
            Name = "repairvehicle";
            Arguments = "<vehicle>";
            Description = "Fully repairs a vehicle.";
            MinimumArguments = 1;
            MaximumArguments = 1;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TemplateObject.Basic_For
            };
        }

        public static void Execute(CommandQueue queue, CommandEntry entry)
        {
            VehicleTag veh = VehicleTag.For(entry.GetArgumentObject(queue, 0));
            if (veh == null)
            {
                queue.HandleError(entry, "Null vehicle!");
                return;
            }
            veh.Internal.Repair();
            if (entry.ShouldShowGood(queue))
            {
                entry.Good(queue, "Vehicle fully repaired!");
            }
        }
    }
}
