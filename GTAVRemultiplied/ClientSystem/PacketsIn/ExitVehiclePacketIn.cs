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
            int sped = BitConverter.ToInt32(data, 0);
            Ped ped = new Ped(ClientConnectionScript.ServerToClientPed[sped]);
            ClientConnectionScript.ServerPedKnownPosition[sped].InVehicle = false;
            if (ped.CurrentVehicle != null)
            {
                ped.Task.LeaveVehicle(ped.CurrentVehicle, false);
                ped.Task.LeaveVehicle();
            }
            return true;
        }
    }
}
