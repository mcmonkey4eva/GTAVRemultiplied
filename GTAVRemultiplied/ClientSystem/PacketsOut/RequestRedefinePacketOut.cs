using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTAVRemultiplied.ServerSystem;

namespace GTAVRemultiplied.ClientSystem.PacketsOut
{
    public class RequestRedefinePacketOut : AbstractPacketOut
    {
        public RequestRedefinePacketOut(ObjectType type, int obj)
        {
            ID = ClientToServerPacket.REQUEST_REDEFINE;
            Data = new byte[1 + 4];
            Data[0] = (byte)type;
            BitConverter.GetBytes(obj).CopyTo(Data, 1);
        }
    }
}
