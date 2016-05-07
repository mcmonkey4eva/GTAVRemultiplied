using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class RemovePedPacketOut : AbstractPacketOut
    {
        public RemovePedPacketOut(int ped)
        {
            ID = ServerToClientPacket.REMOVE_PED;
            Data = BitConverter.GetBytes(ped);
        }
    }
}
