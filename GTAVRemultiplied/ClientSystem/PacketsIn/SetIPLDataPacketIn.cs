using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class SetIPLDataPacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            GTAVUtilities.SetIPLData(data);
            return true;
        }
    }
}
