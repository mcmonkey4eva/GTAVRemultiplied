using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class UpdatePropPacketOut : AbstractPacketOut
    {
        public UpdatePropPacketOut(Prop prop)
        {
            ID = ServerToClientPacket.UPDATE_PROP;
            Data = new byte[4 + 12 + 12 + 12 + 1 + 12 + 4];
            BitConverter.GetBytes(prop.Handle).CopyTo(Data, 0);
            BitConverter.GetBytes(prop.Position.X).CopyTo(Data, 4);
            BitConverter.GetBytes(prop.Position.Y).CopyTo(Data, 4 + 4);
            BitConverter.GetBytes(prop.Position.Z).CopyTo(Data, 4 + 8);
            BitConverter.GetBytes(prop.Velocity.X).CopyTo(Data, 4 + 12);
            BitConverter.GetBytes(prop.Velocity.Y).CopyTo(Data, 4 + 12 + 4);
            BitConverter.GetBytes(prop.Velocity.Z).CopyTo(Data, 4 + 12 + 8);
            BitConverter.GetBytes(prop.Quaternion.X).CopyTo(Data, 4 + 12 + 12);
            BitConverter.GetBytes(prop.Quaternion.Y).CopyTo(Data, 4 + 12 + 12 + 4);
            BitConverter.GetBytes(prop.Quaternion.Z).CopyTo(Data, 4 + 12 + 12 + 8);
            Data[4 + 12 + 12 + 12] = (byte)(prop.IsDead ? 1 : 0);
            Vector3 rvel = GTAVUtilities.GetRotationVelocity(prop);
            BitConverter.GetBytes(rvel.X).CopyTo(Data, 4 + 12 + 12 + 12 + 1);
            BitConverter.GetBytes(rvel.Y).CopyTo(Data, 4 + 12 + 12 + 12 + 1 + 4);
            BitConverter.GetBytes(rvel.Z).CopyTo(Data, 4 + 12 + 12 + 12 + 1 + 8);
            BitConverter.GetBytes(prop.Quaternion.W).CopyTo(Data, 4 + 12 + 12 + 12 + 1 + 12);
        }
    }
}
