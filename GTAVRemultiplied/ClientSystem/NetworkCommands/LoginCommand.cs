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
using GTAVRemultiplied.SharedSystems;

namespace GTAVRemultiplied.ClientSystem.NetworkCommands
{
    public class LoginCommand : AbstractCommand
    {
        // TODO: Meta!

        public LoginCommand()
        {
            Name = "login";
            Arguments = "<username> <password>";
            Description = "Logs in to an account.";
            MinimumArguments = 2;
            MaximumArguments = 2;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TextTag.For,
                TextTag.For
            };
        }

        public static void Execute(CommandQueue queue, CommandEntry entry)
        {
            string username = entry.GetArgument(queue, 0);
            string password = entry.GetArgument(queue, 1);
            if (entry.ShouldShowGood(queue))
            {
                entry.Good(queue, "Attempting login...");
            }
            AccountHelper.GlobalLoginAttempt(username, password, GTAVFrenetic.Schedule);
        }
    }
}
