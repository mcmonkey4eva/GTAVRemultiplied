﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied
{
    public enum ClientToServerPacket : byte
    {
        SELF_UPDATE = 0,
        FIRED_SHOT = 1,
        JUMP = 2
    }

    public enum ServerToClientPacket : byte
    {
        PLAYER_UPDATE = 0,
        FIRED_SHOT = 1,
        JUMP = 2,
        ADD_VEHICLE = 3,
        REMOVE_VEHICLE = 4,
        UPDATE_VEHICLE = 5
    }
}
