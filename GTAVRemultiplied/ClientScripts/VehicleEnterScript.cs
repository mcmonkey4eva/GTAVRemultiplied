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

    public Vector3 PosFor(Vehicle veh, VehicleSeat seat)
    {
        if (veh.IsSeatFree(seat))
        {
            // TODO: Less hilarious position finder?
            Ped p = veh.CreatePedOnSeat(seat, PedHash.DeadHooker);
            Vector3 pos = p.GetBoneCoord(Bone.IK_Head);
            p.Delete();
            return pos;
        }
        else
        {
            return veh.GetPedOnSeat(seat).GetBoneCoord(Bone.IK_Head);
        }
    }

    public void GetClosestSeat(Vehicle veh, ref float closest, ref Vector3 closepos, ref string seatName)
    {
        foreach (Tuple<VehicleSeat, string> seatd in seatOptions)
        {
            if (veh.IsSeatFree(seatd.Item1))
            {
                Vector3 pos = PosFor(veh, seatd.Item1);
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

    public static bool UseFancy = true;

    private void VehicleEnterScript_Tick(object sender, EventArgs e)
    {
        try
        {
            if ((!ClientConnectionScript.Connected && !GTAVFreneticServer.Enabled) || !UseFancy)
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
                Game.Player.SetMayNotEnterAnyVehicleThisFrame();
                TapDetector += GTAVFrenetic.cDelta;
                bool held = TapDetector > 1.0;
                float closest = 8f * 8f;
                getinto = null;
                Vector3 closepos = Vector3.Zero;
                Vector3 playerpos = Game.Player.Character.Position;
                string seatName = null;
                foreach (Vehicle veh in World.GetAllVehicles())
                {
                    Vector3 min;
                    Vector3 max;
                    veh.Model.GetDimensions(out min, out max);
                    min -= new Vector3(7, 7, 7);
                    max += new Vector3(7, 7, 7);
                    min += veh.Position;
                    max += veh.Position;
                    if (!(playerpos.X > min.X && playerpos.Y > min.Y && playerpos.Z > min.Z
                        && playerpos.X < max.X && playerpos.Y < max.Y && playerpos.Z < max.Z))
                    {
                        continue;
                    }
                    bool driverOpen = veh.IsSeatFree(VehicleSeat.Driver);
                    if (!driverOpen)
                    {
                        Ped driver = veh.GetPedOnSeat(VehicleSeat.Driver);
                        if (driver == null || driver.AttachedBlips.Length == 0) // TODO: better player/important check system?
                        {
                            driverOpen = true;
                        }
                    }
                    if (held)
                    {
                        if (driverOpen)
                        {
                            GetClosestSeat(veh, ref closest, ref closepos, ref seatName);
                        }
                        else
                        {
                            Vector3 pos = PosFor(veh, VehicleSeat.Driver);
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
                            Vector3 pos = PosFor(veh, VehicleSeat.Driver);
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
                else
                {
                    ClientConnectionScript.Text3D(Game.Player.Character.Position + new Vector3(0, 0, 0.5f), "<None>", System.Drawing.Color.Yellow, 5f);
                }
            }
            else
            {
                TapDetector = 0;
                if (getinto != null)
                {
                    if (!getinto.IsSeatFree(spot))
                    {
                        getinto.GetPedOnSeat(spot).CanBeDraggedOutOfVehicle = true;
                        if (getinto.GetPedOnSeat(spot).IsDead)
                        {
                            // TODO: How do we move out bodies reasonably? Ideally would be a player-pulls-the-body-out animation!
                            getinto.GetPedOnSeat(spot).Task.LeaveVehicle(LeaveVehicleFlags.WarpOut);
                            Game.Player.Character.Task.EnterVehicle(getinto, VehicleSeat.Any, -1, 2);
                        }
                        else
                        {
                            getinto.GetPedOnSeat(spot).Task.LeaveVehicle(LeaveVehicleFlags.BailOut);
                        }
                    }
                    else
                    {
                        Game.Player.Character.Task.EnterVehicle(getinto, spot, -1, 2);
                    }
                    getinto = null;
                }
                else
                {
                    Game.Player.SetMayNotEnterAnyVehicleThisFrame();
                }
            }
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
        }
    }
}
