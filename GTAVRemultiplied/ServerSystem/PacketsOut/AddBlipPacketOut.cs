using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class AddBlipPacketOut : AbstractPacketOut
    {
        public AddBlipPacketOut(Ped ped, BlipSprite sprite, BlipColor color) // TODO: generic location/entity/etc blips?
        {
            ID = ServerToClientPacket.ADD_BLIP;
            Data = new byte[4 + 4 + 4];
            BitConverter.GetBytes(ped.Handle).CopyTo(Data, 0);
            BitConverter.GetBytes((int)sprite).CopyTo(Data, 4);
            BitConverter.GetBytes((int)color).CopyTo(Data, 8);
        }
    }
}
