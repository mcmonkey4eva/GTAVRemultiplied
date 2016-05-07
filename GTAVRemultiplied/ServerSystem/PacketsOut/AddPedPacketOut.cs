using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class AddPedPacketOut : AbstractPacketOut
    {
        public AddPedPacketOut(Ped ped)
        {
            ID = ServerToClientPacket.ADD_PED;
            Data = new byte[4 + 4 + 12 + 4];
            BitConverter.GetBytes(ped.Handle).CopyTo(Data, 0);
            BitConverter.GetBytes(ped.Model.Hash).CopyTo(Data, 4);
            Vector3 pos = ped.Position;
            BitConverter.GetBytes(pos.X).CopyTo(Data, 4 + 4);
            BitConverter.GetBytes(pos.Y).CopyTo(Data, 4 + 4 + 4);
            BitConverter.GetBytes(pos.Z).CopyTo(Data, 4 + 4 + 8);
            BitConverter.GetBytes(ped.Heading).CopyTo(Data, 4 + 4 + 12);
        }
    }
}
