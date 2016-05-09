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
    public class SetModelPacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 4 + 4)
            {
                return false;
            }
            Ped ped = new Ped(ClientConnectionScript.ServerToClientPed[BitConverter.ToInt32(data, 4)]);
            Model mod = new Model(BitConverter.ToInt32(data, 0));
            Vector3 pos = ped.Position;
            float heading = ped.Heading;
            int hand = ped.Handle;
            ped.Delete();
            ped = World.CreatePed(mod, pos, heading);
            if (ped != null)
            {
                int serverPed = ClientConnectionScript.ClientToServerPed[hand];
                ClientConnectionScript.ServerToClientPed.Remove(serverPed);
                ClientConnectionScript.ClientToServerPed.Remove(hand);
                ClientConnectionScript.ServerToClientPed[serverPed] = ped.Handle;
                ClientConnectionScript.ClientToServerPed[ped.Handle] = serverPed;
                ped.IsPersistent = true;
                ped.IsInvincible = true;
                ped.BlockPermanentEvents = true;
                ped.Task.ClearAllImmediately();
                ped.SetDefaultClothes();
                PedInfo pinfo = ClientConnectionScript.ServerPedKnownPosition[serverPed];
                if (pinfo.hasBlip)
                {
                    Blip blip = ped.AddBlip();
                    blip.Sprite = pinfo.blipSprite;
                    blip.Color = pinfo.blipColor;
                }
            }
            else
            {
                Log.Message("Warning", "Null character spawned: " + mod.Hash, 'Y');
            }
            return true;
        }
    }
}
