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
        public JumpPacketOut(Ped character)
        {
            // TODO: Player ID!
            ID = ServerToClientPacket.JUMP;
            Data = BitConverter.GetBytes(character.Handle);
        }
    }
}
