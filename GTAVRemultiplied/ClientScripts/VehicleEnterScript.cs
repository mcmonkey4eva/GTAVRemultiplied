using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTAVRemultiplied.ClientSystem;
using GTAVRemultiplied.ClientSystem.TagBases;
using GTAVRemultiplied;
using GTAVRemultiplied.ServerSystem;

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
        // Intentionally drop these two:
        //new Tuple<VehicleSeat, string>(VehicleSeat.RightFront, "Right-Front"),
        //new Tuple<VehicleSeat, string>(VehicleSeat.LeftFront, "Left-Front"),
        new Tuple<VehicleSeat, string>(VehicleSeat.RightRear, "Right-Rear"),
        new Tuple<VehicleSeat, string>(VehicleSeat.LeftRear, "Left-Rear"),
        new Tuple<VehicleSeat, string>(VehicleSeat.ExtraSeat1, "Extra 1"),
        new Tuple<VehicleSeat, string>(VehicleSeat.ExtraSeat2, "Extra 2"),
        new Tuple<VehicleSeat, string>(VehicleSeat.ExtraSeat3, "Extra 3"),
        new Tuple<VehicleSeat, string>(VehicleSeat.ExtraSeat4, "Extra 4")
        // TODO: other extras, probably.
    };

    Vehicle getinto = null;
    VehicleSeat spot = VehicleSeat.None;
    
    public static double LastVehicle = 0;

    private void VehicleEnterScript_Tick(object sender, EventArgs e)
    {
        if (!ClientConnectionScript.Connected && !GTAVFreneticServer.Enabled)
        {
            return;
        }
        if (Game.Player.Character.IsInVehicle())
        {
            LastVehicle = GTAVFrenetic.GlobalTickTime;
            return;
        }
        if (GTAVFrenetic.GlobalTickTime - LastVehicle < 3)
        {
            return;
        }
        if (ControlTagBase.ControlDown(Control.Enter))
        {
            float closest = 8 * 8;
            getinto = null;
            Vector3 closepos = Vector3.Zero;
            Vector3 playerpos = Game.Player.Character.Position;
            string seatName = null;
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
                        // TODO: Less hilarious position finder?
                        Ped p = veh.CreatePedOnSeat(seatd.Item1, PedHash.DeadHooker);
                        Vector3 pos = p.GetBoneCoord(Bone.IK_Head);
                        p.Delete();
                        float dist = pos.DistanceToSquared2D(Game.Player.Character.Position);
                        if (dist < closest)
                        {
                            closest = dist;
                            getinto = veh;
                            spot = seatd.Item1;
                            closepos = pos;
                            seatName = seatd.Item2;
                        }
                        if (DebugPositionScript.Enabled)
                        {
                            ClientConnectionScript.Text3D(pos, "[" + seatd.Item2 + "]");
                        }
                    }
                }
            }
            if (getinto != null && seatName != null)
            {
                ClientConnectionScript.Text3D(closepos + new Vector3(0, 0, -0.5f), seatName, System.Drawing.Color.Yellow);
            }
        }
        else if (getinto != null)
        {
            Game.Player.Character.Task.EnterVehicle(getinto, spot);
            getinto = null;
        }
    }
}
