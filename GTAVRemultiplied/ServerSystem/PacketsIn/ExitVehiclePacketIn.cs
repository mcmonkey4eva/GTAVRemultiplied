using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAVRemultiplied.ServerSystem.PacketsIn
{
    public class ExitVehiclePacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(GTAVServerClientConnection client, byte[] data)
        {
            if (data.Length != 0)
            {
                return false;
            }
            if (client.Character.CurrentVehicle != null)
            {
                client.Character.Task.LeaveVehicle(client.Character.CurrentVehicle, false);
            }
            return true;
        }
    }
}
