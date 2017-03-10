using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTAVRemultiplied.ClientSystem.PacketsOut;

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
            if (!ClientConnectionScript.ServerToClientPed.TryGetValue(ped, out int tped))
            {
                ClientConnectionScript.SendPacket(new RequestRedefinePacketOut(ObjectType.PED, ped));
                return true;
            }
            Ped p = new Ped(tped);
            if (!ClientConnectionScript.ServerToClientVehicle.TryGetValue(veh, out int vid))
            {
                ClientConnectionScript.SendPacket(new RequestRedefinePacketOut(ObjectType.VEHICLE, veh));
                return true;
            }
            Vehicle v = new Vehicle(vid);
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
            return true;
        }
    }
}
