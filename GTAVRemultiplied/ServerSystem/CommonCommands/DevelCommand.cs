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
using GTAVRemultiplied.ServerSystem.PacketsOut;

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
                case "quickNPC":
                    PedHash pmod;
                    if (Enum.TryParse(arg2.ToString(), true, out pmod))
                    {
                        Ped p = World.CreatePed(pmod, Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5);
                        p.Position += new Vector3(0, 0, -p.HeightAboveGround);
                        if (entry.ShouldShowGood(queue))
                        {
                            entry.Good(queue, "NPC spawned!");
                        }
                    }
                    else
                    {
                        queue.HandleError(entry, "Invalid input!");
                    }
                    break;
                case "quickVehicle":
                    VehicleHash veh;
                    if (Enum.TryParse(arg2.ToString(), true, out veh))
                    {
                        Vehicle v = World.CreateVehicle(veh, Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5);
                        v.Position += new Vector3(0, 0, -v.HeightAboveGround);
                        queue.SetVariable("devel_quick_vehicle", new VehicleTag(v));
                        if (entry.ShouldShowGood(queue))
                        {
                            entry.Good(queue, "Vehicle spawned!");
                        }
                    }
                    else
                    {
                        queue.HandleError(entry, "Invalid input!");
                    }
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
                case "clothesSet":
                    ListTag list = new ListTag();
                    for (int i = 0; i < 12; i++)
                    {
                        list.ListEntries.Add(new TextTag(i + "->" + Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, Game.Player.Character.Handle, i)));
                    }
                    entry.Info(queue, "Clothing was..." + list.ToString());
                    int choice = (int)IntegerTag.TryFor(arg2).Internal;
                    for (int i = 0; i < 12; i++)
                    {
                        ModelEnforcementScript.SetClothing(Game.Player.Character, i, choice, 0);
                    }
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Clothing set!");
                    }
                    break;
                case "addRegion":
                    if (arg2.ToString() == "yankton")
                    {
                        GTAVUtilities.SpawnNorthYankton();
                    }
                    else if (arg2.ToString() == "carrier")
                    {
                        GTAVUtilities.SpawnCarrier();
                    }
                    foreach (GTAVServerClientConnection client in GTAVFreneticServer.Connections.Connections)
                    {
                        client.SendPacket(new SetIPLDataPacketOut());
                    }
                    break;
                case "removeRegion":
                    if (arg2.ToString() == "yankton")
                    {
                        GTAVUtilities.RemoveNorthYankton();
                    }
                    else if (arg2.ToString() == "carrier")
                    {
                        GTAVUtilities.RemoveCarrier();
                    }
                    foreach (GTAVServerClientConnection client in GTAVFreneticServer.Connections.Connections)
                    {
                        client.SendPacket(new SetIPLDataPacketOut());
                    }
                    break;
                default:
                    queue.HandleError(entry, "Invalid devel command!");
                    break;
            }
        }
    }
}
