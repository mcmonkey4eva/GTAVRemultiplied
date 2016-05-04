using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ServerSystem
{
    public abstract class AbstractPacketIn
    {
        public abstract bool ParseAndExecute(GTAVServerClientConnection client, byte[] data);
    }
}
