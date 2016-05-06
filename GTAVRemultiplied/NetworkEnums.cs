using System;
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
        JUMP = 2,
        ENTER_VEHICLE = 3,
        EXIT_VEHICLE = 4
    }

    public enum ServerToClientPacket : byte
    {
        PLAYER_UPDATE = 0,
        FIRED_SHOT = 1,
        JUMP = 2,
        ENTER_VEHICLE = 3,
        EXIT_VEHICLE = 4,
        ADD_VEHICLE = 5,
        REMOVE_VEHICLE = 6,
        UPDATE_VEHICLE = 7
    }
}
