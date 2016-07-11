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
    class ModVehicleCommand : AbstractCommand
    {
        // TODO: Meta!

        public ModVehicleCommand()
        {
            Name = "modvehicle";
            Arguments = "<vehicle> <option> <value>";
            Description = "Modifies a vehicle.";
            MinimumArguments = 3;
            MaximumArguments = 3;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TemplateObject.Basic_For,
                GetLowText,
                TemplateObject.Basic_For
            };
        }

        public TemplateObject GetLowText(TemplateObject input)
        {
            return new TextTag(input.ToString().ToLowerFast());
        }

        public override void Execute(CommandQueue queue, CommandEntry entry)
        {
            VehicleTag veh = VehicleTag.For(entry.GetArgumentObject(queue, 0));
            if (veh == null)
            {
                queue.HandleError(entry, "Null vehicle!");
                return;
            }
            string option = entry.GetArgument(queue, 1).ToLowerFast();
            TemplateObject val = entry.GetArgumentObject(queue, 2);
            switch (option)
            {
                case "color_primary":
                    {
                        string vs = val.ToString();
                        if (vs == "")
                        {
                            veh.Internal.ClearCustomPrimaryColor();
                        }
                        else
                        {
                            // TODO: Color tag!
                            Vector3 col = GTAVUtilities.StringToVector(vs);
                            veh.Internal.CustomPrimaryColor = System.Drawing.Color.FromArgb((int)(col.X * 255), (int)(col.Y * 255), (int)(col.Z * 255));
                        }
                    }
                    break;
                case "color_secondary":
                    {
                        string vs = val.ToString();
                        if (vs == "")
                        {
                            veh.Internal.ClearCustomSecondaryColor();
                        }
                        else
                        {
                            // TODO: Color tag!
                            Vector3 col = GTAVUtilities.StringToVector(vs);
                            veh.Internal.CustomSecondaryColor = System.Drawing.Color.FromArgb((int)(col.X * 255), (int)(col.Y * 255), (int)(col.Z * 255));
                        }
                    }
                    break;
                case "color_trim":
                    veh.Internal.TrimColor = (VehicleColor)Enum.Parse(typeof(VehicleColor), val.ToString(), true);
                    break;
                case "color_rim":
                    veh.Internal.RimColor = (VehicleColor)Enum.Parse(typeof(VehicleColor), val.ToString(), true);
                    break;
                case "color_shine":
                    veh.Internal.PearlescentColor = (VehicleColor)Enum.Parse(typeof(VehicleColor), val.ToString(), true);
                    break;
                case "invincible":
                    veh.Internal.IsInvincible = BooleanTag.TryFor(val).Internal;
                    break;
                case "window_tint":
                    // TODO: Functionality here?
                    veh.Internal.WindowTint = (VehicleWindowTint)Enum.Parse(typeof(VehicleWindowTint), val.ToString(), true);
                    break;
                default:
                    queue.HandleError(entry, "Unknown option!");
                    return;
            }
            if (entry.ShouldShowGood(queue))
            {
                entry.Good(queue, "Vehicle modified!");
            }
        }
    }
}
