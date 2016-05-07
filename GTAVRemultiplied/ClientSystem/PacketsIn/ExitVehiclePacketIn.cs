using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class ExitVehiclePacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 4)
            {
                return false;
            }
            Ped ped = new Ped(ClientConnectionScript.ServerToClientPed[BitConverter.ToInt32(data, 0)]);
            if (ped.CurrentVehicle != null)
            {
                ped.Task.LeaveVehicle(ped.CurrentVehicle, false);
            }
            return true;
        }
    }
}
