using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTAVRemultiplied.ServerSystem.PacketsOut;

namespace GTAVRemultiplied.ServerSystem.PacketsIn
{
    public class RequestRedefinePacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(GTAVServerClientConnection client, byte[] data)
        {
            if (data.Length != 1 + 4)
            {
                return false;
            }
            ObjectType ot = (ObjectType)data[0];
            int objd = BitConverter.ToInt32(data, 1);
            if (ot == ObjectType.VEHICLE)
            {
                if (World.GetAllVehicles().Any((v) => v.Handle == objd))
                {
                    client.SendPacket(new AddVehiclePacketOut(new Vehicle(objd)));
                }
            }
            else if (ot == ObjectType.PED)
            {
                if (World.GetAllPeds().Any((p) => p.Handle == objd))
                {
                    // TODO: Other data as needed!
                    client.SendPacket(new AddPedPacketOut(new Ped(objd)));
                }
            }
            return true;
        }
    }
}
