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
        public AddBlipPacketOut(Ped ped, BlipSprite sprite, BlipColor color, string name) // TODO: generic location/entity/etc blips?
        {
            ID = ServerToClientPacket.ADD_BLIP;
            byte[] nameb = GTAVUtilities.Enc.GetBytes(name ?? "");
            Data = new byte[4 + 4 + 4 + 4 + nameb.Length];
            BitConverter.GetBytes(ped.Handle).CopyTo(Data, 0);
            BitConverter.GetBytes((int)sprite).CopyTo(Data, 4);
            BitConverter.GetBytes((int)color).CopyTo(Data, 8);
            BitConverter.GetBytes(nameb.Length).CopyTo(Data, 12);
            nameb.CopyTo(Data, 16);
        }
    }
}
