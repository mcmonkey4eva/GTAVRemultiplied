﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class RemovePedPacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 4)
            {
                return false;
            }
            int id = BitConverter.ToInt32(data, 0);
            int pd;
            if (ClientConnectionScript.ServerToClientPed.TryGetValue(id, out pd))
            {
                Ped ped = new Ped(pd);
                ped.Delete();
                ClientConnectionScript.ServerToClientPed.Remove(id);
                ClientConnectionScript.ClientToServerPed.Remove(pd);
            }
            else
            {
                Log.Message("Warning", "Unknown character removed!", 'Y');
            }
            return true;
        }
    }
}