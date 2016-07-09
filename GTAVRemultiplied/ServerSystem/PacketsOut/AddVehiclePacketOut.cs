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
            int ind = 4 + 4 + 12 + 4 + 8 + 2 + 4;
            Data = new byte[ind + 5];
            BitConverter.GetBytes(vehicle.Handle).CopyTo(Data, 0);
            BitConverter.GetBytes(vehicle.Model.Hash).CopyTo(Data, 4);
            Vector3 pos = vehicle.Position;
            BitConverter.GetBytes(pos.X).CopyTo(Data, 4 + 4);
            BitConverter.GetBytes(pos.Y).CopyTo(Data, 4 + 4 + 4);
            BitConverter.GetBytes(pos.Z).CopyTo(Data, 4 + 4 + 8);
            BitConverter.GetBytes(vehicle.Heading).CopyTo(Data, 4 + 4 + 12);
            string numPlate = vehicle.LicensePlate;
            for (int i = 0; i < numPlate.Length; i++)
            {
                Data[4 + 4 + 12 + 4 + i] = (byte)numPlate[i];
            }
            for (int i = numPlate.Length; i < 8; i++)
            {
                Data[4 + 4 + 12 + 4 + i] = (byte)' ';
            }
            Data[4 + 4 + 12 + 4 + 8] = (byte)vehicle.LicensePlateType;
            Data[4 + 4 + 12 + 4 + 8 + 1] = 0; // Reserved: NumberPlateMounting setter?
            BitConverter.GetBytes(vehicle.ColorCombination).CopyTo(Data, 4 + 4 + 12 + 4 + 8 + 2);
            Data[ind] = (byte)vehicle.PrimaryColor;
            Data[ind + 1] = (byte)vehicle.SecondaryColor;
            Data[ind + 2] = (byte)vehicle.PearlescentColor;
            Data[ind + 3] = (byte)vehicle.TrimColor;
            Data[ind + 4] = (byte)vehicle.RimColor;
        }
    }
}
