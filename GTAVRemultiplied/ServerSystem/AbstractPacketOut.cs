using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ServerSystem
{
    public abstract class AbstractPacketOut
    {
        public byte ID;

        public byte[] Data;
    }
}
