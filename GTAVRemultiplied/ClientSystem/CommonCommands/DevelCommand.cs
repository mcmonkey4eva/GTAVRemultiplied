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
using GTAVRemultiplied.SharedSystems;

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
                    // EG: switchCharacter DeadHooker
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
                case "spawnNY":
                    GTAVUtilities.SpawnNorthYankton();
                    break;
                case "spawnHC":
                    GTAVUtilities.SpawnCarrier();
                    break;
                case "removeNY":
                    GTAVUtilities.RemoveNorthYankton();
                    break;
                case "removeHC":
                    GTAVUtilities.RemoveCarrier();
                    break;
                case "getWeapons":
                    foreach (WeaponHash hash in ModelEnforcementScript.hashes)
                    {
                        Game.Player.Character.Weapons.Give(hash, 999, false, true);
                    }
                    break;
                case "testJerkoff":
                    Function.Call(Hash.REQUEST_ANIM_DICT, "SWITCH@TREVOR@JERKING_OFF");
                    Function.Call(Hash.TASK_PLAY_ANIM, Game.Player.Character.Handle, "SWITCH@TREVOR@JERKING_OFF", "trev_jerking_off_loop",
                        8f, 1f, 5000, 1, 1f, false, false, false);
                    break;
                    // EG: quickShot RPG
                case "quickShot":
                    Vector3 playerpos = Game.Player.Character.Position;
                    Vector3 playertarget = Game.Player.Character.Position + GameplayCamera.Direction * 50;
                    WeaponHash weap = (WeaponHash)Enum.Parse(typeof(WeaponHash), arg2.ToString(), true);
                    Function.Call(Hash.SHOOT_SINGLE_BULLET_BETWEEN_COORDS, playerpos.X, playerpos.Y, playerpos.Z, playertarget.X, playertarget.Y, playertarget.Z, Game.Player.Character.Health, true,
                        (uint)weap, Game.Player.Character.Handle, true, true, -1);
                    break;
                case "quickSpin":
                    entry.Good(queue, "Your rotation velocity was: " + GTAVUtilities.GetRotationVelocity(Game.Player.Character.CurrentVehicle));
                    GTAVUtilities.SetRotationVelocity(Game.Player.Character.CurrentVehicle, GTAVUtilities.StringToVector(arg2.ToString()));
                    break;
                case "spawnRandomYachts":
                    Random rand = new Random();
                    for (int i = 1; i <= 12; i++)
                    {
                        YachtHelper.SpawnYacht(i, rand.Next(1, 3), rand.Next(100) > 50, lightOpts[rand.Next(lightOpts.Length)]);
                    }
                    break;
                default:
                    queue.HandleError(entry, "What?");
                    return;
            }
        }

        static string[] lightOpts = new string[] { "1a", "1b", "1c", "1d", "2a", "2b", "2c", "2d"};
    }
}
