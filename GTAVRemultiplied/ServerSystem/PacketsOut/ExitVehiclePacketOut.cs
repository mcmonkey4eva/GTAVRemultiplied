﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class ExitVehiclePacketOut : AbstractPacketOut
    {
        public ExitVehiclePacketOut(Ped ped)
        {
            ID = ServerToClientPacket.EXIT_VEHICLE;
            Data = BitConverter.GetBytes(ped.Handle);
        }
    }
}
