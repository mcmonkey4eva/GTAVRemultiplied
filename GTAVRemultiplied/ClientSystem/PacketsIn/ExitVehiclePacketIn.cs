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
            int tped;
            if (!ClientConnectionScript.ServerToClientPed.TryGetValue(sped, out tped))
            {
                return true; // TODO: Maybe send a 'request redefine' packet?
            }
            Ped ped = new Ped(tped);
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
