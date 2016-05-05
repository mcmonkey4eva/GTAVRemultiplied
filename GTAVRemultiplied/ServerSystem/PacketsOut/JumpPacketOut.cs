using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class JumpPacketOut : AbstractPacketOut
    {
        public JumpPacketOut(Player character) // TODO: GTAV-RMP Player object rather than GTA Player?
        {
            // TODO: Player ID!
            ID = ServerToClientPacket.JUMP;
            Data = new byte[0];
        }
    }
}
