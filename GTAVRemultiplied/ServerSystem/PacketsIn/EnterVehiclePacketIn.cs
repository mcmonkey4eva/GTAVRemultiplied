using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAVRemultiplied.ServerSystem.PacketsIn
{
    public class EnterVehiclePacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(GTAVServerClientConnection client, byte[] data)
        {
            if (data.Length != 5)
            {
                return false;
            }
            int veh = BitConverter.ToInt32(data, 0);
            // TODO: Validate vehicle ID!
            // TODO: Validate the seat!
            // TODO: Make sure the character can validly move into this vehicle, and isn't using vehicles to teleport!
            Vehicle v = new Vehicle(veh);
            VehicleSeat seat = (VehicleSeat)(data[4] - 3);
            if (!v.IsSeatFree(seat))
            {
                Ped problem = v.GetPedOnSeat(seat);
                if (problem.Handle != Game.Player.Character.Handle && problem.AttachedBlip == null)
                {
                    problem.Task.LeaveVehicle(LeaveVehicleFlags.WarpOut);
                }
                else
                {
                    return true;
                }
            }
            client.Character.SetIntoVehicle(v, seat);
            client.InVehicle = true;
            client.lRot = v.Quaternion;
            client.lPos = v.Position;
            return true;
        }
    }
}
