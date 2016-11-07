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
    
    public double LastVehicle = 0;

    public double TapDetector = 0;

    public Vector3? PosFor(Vehicle veh, VehicleSeat seat)
    {
        if (veh.IsSeatFree(seat))
        {
            // TODO: Less hilarious position finder?
            Ped p = veh.CreatePedOnSeat(seat, PedHash.DeadHooker);
            Vector3 pos = p.GetBoneCoord(Bone.IK_Head);
            p.Delete();
            return pos;
        }
        return null;
    }

    public void GetClosestSeat(Vehicle veh, ref float closest, ref Vector3 closepos, ref string seatName)
    {
        foreach (Tuple<VehicleSeat, string> seatd in seatOptions)
        {
            Vector3? pos = PosFor(veh, seatd.Item1);
            if (pos.HasValue)
            {
                float dist = pos.Value.DistanceToSquared2D(Game.Player.Character.Position);
                if (dist < closest)
                {
                    closest = dist;
                    getinto = veh;
                    spot = seatd.Item1;
                    closepos = pos.Value;
                    seatName = seatd.Item2;
                }
                if (DebugPositionScript.Enabled)
                {
                    ClientConnectionScript.Text3D(pos.Value, "[" + seatd.Item2 + "]");
                }
            }
        }
    }

    private void VehicleEnterScript_Tick(object sender, EventArgs e)
    {
        try
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
            Game.Player.SetMayNotEnterAnyVehicleThisFrame();
            if (ControlTagBase.ControlDown(Control.Enter))
            {
                TapDetector += GTAVFrenetic.cDelta;
                bool held = TapDetector > 1.0;
                float closest = 8f * 8f;
                getinto = null;
                Vector3 closepos = Vector3.Zero;
                Vector3 playerpos = Game.Player.Character.Position;
                string seatName = null;
                foreach (Vehicle veh in World.GetAllVehicles())
                {
                    if (veh.Position.DistanceToSquared(playerpos) > 7f * 7f)
                    {
                        continue;
                    }
                    bool driverOpen = veh.IsSeatFree(VehicleSeat.Driver);
                    if (held)
                    {
                        if (driverOpen)
                        {
                            GetClosestSeat(veh, ref closest, ref closepos, ref seatName);
                        }
                        else
                        {
                            Vector3 pos = veh.GetPedOnSeat(VehicleSeat.Driver).GetBoneCoord(Bone.IK_Head);
                            float dist = pos.DistanceToSquared2D(Game.Player.Character.Position);
                            if (dist < closest)
                            {
                                closest = dist;
                                getinto = veh;
                                spot = VehicleSeat.Driver;
                                closepos = pos;
                                seatName = "Driver";
                            }
                        }
                    }
                    else
                    {
                        if (driverOpen)
                        {
                            Vector3? pos = PosFor(veh, VehicleSeat.Driver);
                            if (pos.HasValue)
                            {
                                float dist = pos.Value.DistanceToSquared2D(Game.Player.Character.Position);
                                if (dist < closest)
                                {
                                    closest = dist;
                                    getinto = veh;
                                    spot = VehicleSeat.Driver;
                                    closepos = pos.Value;
                                    seatName = "Driver";
                                }
                            }
                        }
                        else
                        {
                            GetClosestSeat(veh, ref closest, ref closepos, ref seatName);
                        }
                    }
                }
                if (getinto != null && seatName != null)
                {
                    ClientConnectionScript.Text3D(closepos + new Vector3(0, 0, -0.5f), seatName, System.Drawing.Color.Yellow, 5f);
                    World.DrawSpotLight(closepos + new Vector3(0, 0, 3), new Vector3(0, 0, -1), System.Drawing.Color.Yellow, 10f, 3f, 1f, 20f, 1f);
                }
            }
            else
            {
                TapDetector = 0;
                if (getinto != null)
                {
                    Game.Player.Character.Task.EnterVehicle(getinto, spot);
                    getinto = null;
                }
            }
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
        }
    }
}
