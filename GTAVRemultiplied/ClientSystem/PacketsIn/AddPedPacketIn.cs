using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class AddPedPacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 4 + 4 + 12 + 4)
            {
                return false;
            }
            int id = BitConverter.ToInt32(data, 0);
            int hash = BitConverter.ToInt32(data, 4);
            Vector3 pos = new Vector3();
            pos.X = BitConverter.ToSingle(data, 4 + 4);
            pos.Y = BitConverter.ToSingle(data, 4 + 4 + 4);
            pos.Z = BitConverter.ToSingle(data, 4 + 4 + 8);
            float heading = BitConverter.ToSingle(data, 4 + 4 + 12);
            Ped ped = World.CreatePed(new Model(hash), pos, heading);
            if (ped == null)
            {
                ped = World.CreatePed(new Model(hash), Game.Player.Character.Position + new Vector3(10, 10, 10), heading);
                if (ped == null)
                {
                    Log.Error("Null character spawned: " + hash + ", " + id + ", " + pos);
                    return true;
                }
            }
            ped.IsPersistent = true;
            ped.IsInvincible = true;
            ped.BlockPermanentEvents = true;
            ped.Task.ClearAllImmediately();
            ped.SetDefaultClothes();
            ClientConnectionScript.ServerToClientPed[id] = ped.Handle;
            ClientConnectionScript.ClientToServerPed[ped.Handle] = id;
            PedInfo pinf = new PedInfo();
            pinf.Character = ped;
            ClientConnectionScript.ServerPedKnownPosition[id] = pinf;
            return true;
        }
    }
}
