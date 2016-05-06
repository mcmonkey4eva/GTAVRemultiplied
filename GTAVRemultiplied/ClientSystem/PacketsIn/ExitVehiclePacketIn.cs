using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class ExitVehiclePacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 0)
            {
                return false;
            }
            if (ClientConnectionScript.Character.CurrentVehicle != null)
            {
                ClientConnectionScript.Character.Task.LeaveVehicle(ClientConnectionScript.Character.CurrentVehicle, false);
            }
            return true;
        }
    }
}
