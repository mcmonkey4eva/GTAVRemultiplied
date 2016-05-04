using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ClientSystem
{
    public abstract class AbstractPacketIn
    {
        public abstract bool ParseAndExecute(byte[] data);
    }
}
