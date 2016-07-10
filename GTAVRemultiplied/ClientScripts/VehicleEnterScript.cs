using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTAVRemultiplied.ClientSystem.TagBases;
using GTAVRemultiplied;

public class VehicleEnterScript : Script
{
    public VehicleEnterScript()
    {
        Tick += VehicleEnterScript_Tick;
    }
    
    List<Tuple<VehicleSeat, string>> seatOptions = new List<Tuple<VehicleSeat, string>>()
    {
        new Tuple<VehicleSeat, string>(VehicleSeat.Driver, "Driver"),
        new Tuple<VehicleSeat, string>(VehicleSeat.Passenger, "Passenger"),
        new Tuple<VehicleSeat, string>(VehicleSeat.RightFront, "Right-Front"),
        new Tuple<VehicleSeat, string>(VehicleSeat.LeftFront, "Left-Front"),
        new Tuple<VehicleSeat, string>(VehicleSeat.RightRear, "Right-Rear"),
        new Tuple<VehicleSeat, string>(VehicleSeat.LeftRear, "Left-Rear"),
        new Tuple<VehicleSeat, string>(VehicleSeat.ExtraSeat1, "Extra 1"),
        new Tuple<VehicleSeat, string>(VehicleSeat.ExtraSeat2, "Extra 2"),
        new Tuple<VehicleSeat, string>(VehicleSeat.ExtraSeat3, "Extra 3"),
        new Tuple<VehicleSeat, string>(VehicleSeat.ExtraSeat4, "Extra 4"),
    };

    Vehicle getinto = null;
    VehicleSeat spot = VehicleSeat.None;

    private void VehicleEnterScript_Tick(object sender, EventArgs e)
    {
        return; // TODO: FIXME!
        if (Game.Player.Character.IsInVehicle())
        {
            return;
        }
        if (ControlTagBase.ControlDown(Control.Enter))
        {
            float closest = 8 * 8;
            getinto = null;
            Vector3 closepos = Vector3.Zero;
            Vector3 playerpos = Game.Player.Character.Position;
            foreach (Vehicle veh in World.GetAllVehicles())
            {
                if (veh.Position.DistanceToSquared(playerpos) > 20f * 20f)
                {
                    continue;
                }
                foreach (Tuple<VehicleSeat, string> seatd in seatOptions)
                {
                    if (veh.IsSeatFree(seatd.Item1))
                    {
                        // TODO: Less hilarious position finder! One that works at all!
                        Ped p = veh.CreatePedOnSeat(seatd.Item1, PedHash.DeadHooker);
                        Vector3 pos = p.Position;
                        p.Delete();
                        float dist = pos.DistanceToSquared2D(Game.Player.Character.Position);
                        if (dist < closest)
                        {
                            closest = dist;
                            getinto = veh;
                            spot = seatd.Item1;
                            closepos = pos;
                        }
                        ClientConnectionScript.Text3D(pos, "[" + seatd.Item2 + "]");
                    }
                }
            }
            if (getinto != null)
            {
                ClientConnectionScript.Text3D(closepos + new Vector3(0, 0, 2), "<RELEASE TO ENTER>");
            }
        }
        else if (getinto != null)
        {
            Game.Player.Character.SetIntoVehicle(getinto, spot);
        }
    }
}
