using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class EnterVehiclePacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 5 + 4)
            {
                return false;
            }
            int veh = BitConverter.ToInt32(data, 0);
            VehicleSeat seat = (VehicleSeat)(data[4] - 3);
            int ped = BitConverter.ToInt32(data, 5);
            int tped;
            if (!ClientConnectionScript.ServerToClientPed.TryGetValue(ped, out tped))
            {
                return true; // TODO: Maybe send a 'request redefine' packet?
            }
            Ped p = new Ped(tped);
            Vehicle v = new Vehicle(ClientConnectionScript.ServerToClientVehicle[veh]);
            ClientConnectionScript.ServerPedKnownPosition[ped].InVehicle = true;
            if (!v.IsSeatFree(seat))
            {
                Ped problem = v.GetPedOnSeat(seat);
                if (problem.Handle != p.Handle)
                {
                    problem.Task.LeaveVehicle(LeaveVehicleFlags.WarpOut);
                }
                else
                {
                    return true;
                }
            }
            p.SetIntoVehicle(v, seat);
            Console.WriteLine("Ped " + ped + " forced into " + veh + ", at spot " + seat + ", confirmed: " + p.IsInVehicle());
            return true;
        }
    }
}
