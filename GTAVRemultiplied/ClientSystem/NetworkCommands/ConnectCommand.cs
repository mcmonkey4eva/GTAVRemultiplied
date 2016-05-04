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

namespace GTAVRemultiplied.ClientSystem.NetworkCommands
{
    class ConnectCommand : AbstractCommand
    {
        // TODO: Meta!

        public ConnectCommand()
        {
            Name = "connect";
            Arguments = "<ip> [port]";
            Description = "Connects to a remote server.";
            MinimumArguments = 1;
            MaximumArguments = 2;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TextTag.For,
                IntegerTag.TryFor
            };
        }

        public override void Execute(CommandQueue queue, CommandEntry entry)
        {
            string ip = entry.GetArgument(queue, 0);
            ushort port = 28010;
            if (entry.Arguments.Count > 1)
            {
                port = (ushort)IntegerTag.TryFor(entry.GetArgumentObject(queue, 0)).Internal;
            }
            if (GTAVFreneticServer.Enabled)
            {
                queue.HandleError(entry, "Already running a server!");
                return;
            }
            if (ClientConnectionScript.Connected)
            {
                queue.HandleError(entry, "Already connected to a server!");
                return;
            }
            ClientConnectionScript.Connect(ip, port);
            if (entry.ShouldShowGood(queue))
            {
                entry.Good(queue, "Connection starting!");
            }
        }
    }
}
