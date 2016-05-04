using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using FreneticScript;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class PlayerUpdatePacketOut : AbstractPacketOut
    {
        public PlayerUpdatePacketOut(Player character) // TODO: GTAV-RMP Player object rather than GTA Player?
        {
            // TODO: Player ID!
            ID = ServerToClientPacket.PLAYER_UPDATE;
            Data = new byte[16];
            BitConverter.GetBytes(character.Character.Position.X).CopyTo(Data, 0);
            BitConverter.GetBytes(character.Character.Position.Y).CopyTo(Data, 4);
            BitConverter.GetBytes(character.Character.Position.Z).CopyTo(Data, 8);
            BitConverter.GetBytes(character.Character.Heading).CopyTo(Data, 12);
        }
    }
}
