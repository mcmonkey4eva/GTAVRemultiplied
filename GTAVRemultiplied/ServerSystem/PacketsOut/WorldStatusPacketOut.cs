using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class WorldStatusPacketOut : AbstractPacketOut
    {
        public WorldStatusPacketOut()
        {
            ID = ServerToClientPacket.WORLD_STATUS;
            Data = new byte[8 + 4 + 4 + 4];
            BitConverter.GetBytes(World.CurrentDayTime.Ticks).CopyTo(Data, 0);
            BitConverter.GetBytes((int)World.Weather).CopyTo(Data, 8);
            BitConverter.GetBytes((int)World.NextWeather).CopyTo(Data, 12);
            BitConverter.GetBytes(World.WeatherTransition).CopyTo(Data, 16);
        }
    }
}
