using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ServerSystem
{
    public class PendingConnection
    {
        public Socket Sock;

        public string Data;
    }
}
