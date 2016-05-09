using GTA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class WorldStatusPacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 8 + 4 + 4 + 4)
            {
                return false;
            }
            long ticks = BitConverter.ToInt64(data, 0);
            Weather weather = (Weather)BitConverter.ToInt32(data, 8);
            Weather nextWeather = (Weather)BitConverter.ToInt32(data, 12);
            float weatherTransition = BitConverter.ToSingle(data, 16);
            World.CurrentDayTime = new TimeSpan(ticks);
            World.Weather = weather;
            World.NextWeather = nextWeather;
            World.WeatherTransition = weatherTransition;
            return true;
        }
    }
}
