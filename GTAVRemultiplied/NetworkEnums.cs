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
        EXIT_VEHICLE = 4,
        REQUEST_MODEL = 5
    }

    public enum ServerToClientPacket : byte
    {
        PLAYER_UPDATE = 0,
        FIRED_SHOT = 1,
        JUMP = 2,
        ENTER_VEHICLE = 3,
        EXIT_VEHICLE = 4,
        SET_MODEL = 5,
        ADD_VEHICLE = 6,
        REMOVE_VEHICLE = 7,
        UPDATE_VEHICLE = 8
    }
}
