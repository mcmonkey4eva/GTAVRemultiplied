using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class AddVehiclePacketOut : AbstractPacketOut
    {
        public AddVehiclePacketOut(Vehicle vehicle, int id)
        {
            ID = ServerToClientPacket.ADD_VEHICLE;
            Data = new byte[24];
            BitConverter.GetBytes(id).CopyTo(Data, 0);
            BitConverter.GetBytes(vehicle.Model.Hash).CopyTo(Data, 4);
            Vector3 aim = vehicle.Position;
            BitConverter.GetBytes(aim.X).CopyTo(Data, 8);
            BitConverter.GetBytes(aim.Y).CopyTo(Data, 12);
            BitConverter.GetBytes(aim.Z).CopyTo(Data, 16);
            BitConverter.GetBytes(vehicle.Heading).CopyTo(Data, 20);
        }
    }
}
