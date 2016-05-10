using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class UpdateVehiclePacketOut : AbstractPacketOut
    {
        public UpdateVehiclePacketOut(Vehicle veh)
        {
            ID = ServerToClientPacket.UPDATE_VEHICLE;
            int ind = 4 + 12 + 12 + 12 + 1 + 12 + 4;
            Data = new byte[ind + 4];
            BitConverter.GetBytes(veh.Handle).CopyTo(Data, 0);
            BitConverter.GetBytes(veh.Position.X).CopyTo(Data, 4);
            BitConverter.GetBytes(veh.Position.Y).CopyTo(Data, 4 + 4);
            BitConverter.GetBytes(veh.Position.Z).CopyTo(Data, 4 + 8);
            BitConverter.GetBytes(veh.Velocity.X).CopyTo(Data, 4 + 12);
            BitConverter.GetBytes(veh.Velocity.Y).CopyTo(Data, 4 + 12 + 4);
            BitConverter.GetBytes(veh.Velocity.Z).CopyTo(Data, 4 + 12 + 8);
            BitConverter.GetBytes(veh.Quaternion.X).CopyTo(Data, 4 + 12 + 12);
            BitConverter.GetBytes(veh.Quaternion.Y).CopyTo(Data, 4 + 12 + 12 + 4);
            BitConverter.GetBytes(veh.Quaternion.Z).CopyTo(Data, 4 + 12 + 12 + 8);
            Data[4 + 12 + 12 + 12] = (byte)(veh.IsDead ? 1 : 0);
            Vector3 rvel = GTAVUtilities.GetRotationVelocity(veh);
            BitConverter.GetBytes(rvel.X).CopyTo(Data, 4 + 12 + 12 + 12 + 1);
            BitConverter.GetBytes(rvel.Y).CopyTo(Data, 4 + 12 + 12 + 12 + 1 + 4);
            BitConverter.GetBytes(rvel.Z).CopyTo(Data, 4 + 12 + 12 + 12 + 1 + 8);
            BitConverter.GetBytes(veh.Quaternion.W).CopyTo(Data, 4 + 12 + 12 + 12 + 1 + 12);
            BitConverter.GetBytes(veh.Speed).CopyTo(Data, ind);
        }
    }
}
