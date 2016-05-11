using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class RemovePropPacketOut : AbstractPacketOut
    {
        public RemovePropPacketOut(int id)
        {
            ID = ServerToClientPacket.REMOVE_PROP;
            Data = BitConverter.GetBytes(id);
        }
    }
}
