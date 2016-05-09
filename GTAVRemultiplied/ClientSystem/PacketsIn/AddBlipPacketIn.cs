using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class AddBlipPacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 4 + 4 + 4)
            {
                return false;
            }
            int serverPed = BitConverter.ToInt32(data, 0);
            Ped ped = new Ped(ClientConnectionScript.ServerToClientPed[serverPed]);
            BlipSprite sprite = (BlipSprite)BitConverter.ToInt32(data, 4);
            BlipColor color = (BlipColor)BitConverter.ToInt32(data, 8);
            Blip blip = ped.AddBlip();
            blip.Sprite = sprite;
            blip.Color = color;
            return true;
        }
    }
}
