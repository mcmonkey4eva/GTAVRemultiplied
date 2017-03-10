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
            if (!ClientConnectionScript.ServerToClientPed.TryGetValue(sped, out int tped))
            {
                return true; // TODO: Maybe send a 'request redefine' packet?
            }
            Ped ped = new Ped(tped);
            ClientConnectionScript.ServerPedKnownPosition[sped].InVehicle = false;
            ped.Task.LeaveVehicle(LeaveVehicleFlags.WarpOut);
            if (ped.CurrentVehicle != null)
            {
                ped.CurrentVehicle.Delete();
            }
            return true;
        }
    }
}
