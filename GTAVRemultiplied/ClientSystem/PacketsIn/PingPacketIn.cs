using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTAVRemultiplied.ClientSystem.PacketsOut;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class PingPacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 1)
            {
                return false;
            }
            ClientConnectionScript.SendPacket(new PingPacketOut(data[0]));
            return true;
        }
    }
}
