using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using FreneticScript;

namespace GTAVRemultiplied.ClientSystem.PacketsOut
{
    public class SelfUpdatePacketOut : AbstractPacketOut
    {
        public SelfUpdatePacketOut()
        {
            ID = ClientToServerPacket.SELF_UPDATE;
            Data = new byte[16];
            BitConverter.GetBytes(Game.Player.Character.Position.X).CopyTo(Data, 0);
            BitConverter.GetBytes(Game.Player.Character.Position.Y).CopyTo(Data, 4);
            BitConverter.GetBytes(Game.Player.Character.Position.Z).CopyTo(Data, 8);
            BitConverter.GetBytes(Game.Player.Character.Heading).CopyTo(Data, 12);
        }
    }
}
