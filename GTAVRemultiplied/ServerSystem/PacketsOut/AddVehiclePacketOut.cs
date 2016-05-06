using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class AddVehiclePacketOut : AbstractPacketOut
    {
        public AddVehiclePacketOut(Vehicle vehicle)
        {
            ID = ServerToClientPacket.ADD_VEHICLE;
            Data = new byte[4 + 4 + 12 + 4];
            BitConverter.GetBytes(vehicle.Handle).CopyTo(Data, 0);
            BitConverter.GetBytes(vehicle.Model.Hash).CopyTo(Data, 4);
            Vector3 pos = vehicle.Position;
            BitConverter.GetBytes(pos.X).CopyTo(Data, 4 + 4);
            BitConverter.GetBytes(pos.Y).CopyTo(Data, 4 + 4 + 4);
            BitConverter.GetBytes(pos.Z).CopyTo(Data, 4 + 4 + 8);
            BitConverter.GetBytes(vehicle.Heading).CopyTo(Data, 4 + 4 + 12);
        }
    }
}
