using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class AddPropPacketOut : AbstractPacketOut
    {
        public AddPropPacketOut(Prop prop)
        {
            ID = ServerToClientPacket.ADD_PROP;
            Data = new byte[4 + 4 + 12];
            BitConverter.GetBytes(prop.Handle).CopyTo(Data, 0);
            BitConverter.GetBytes(prop.Model.Hash).CopyTo(Data, 4);
            Vector3 pos = prop.Position;
            BitConverter.GetBytes(pos.X).CopyTo(Data, 4 + 4);
            BitConverter.GetBytes(pos.Y).CopyTo(Data, 4 + 4 + 4);
            BitConverter.GetBytes(pos.Z).CopyTo(Data, 4 + 4 + 8);
        }
    }
}
