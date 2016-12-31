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
        REQUEST_MODEL = 5,
        REQUEST_REDEFINE = 6
    }

    public enum ObjectType : byte
    {
        VEHICLE = 0,
        PED = 1
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
        UPDATE_VEHICLE = 8,
        ADD_PED = 9,
        REMOVE_PED = 10,
        SET_IPL_DATA = 11,
        WORLD_STATUS = 12,
        ADD_BLIP = 13,
        ADD_PROP = 14,
        REMOVE_PROP = 15,
        UPDATE_PROP = 16
    }

    [Flags]
    public enum PedFlags : byte
    {
        NONE = 0x00,
        DEAD = 0x01,
        RUNNING = 0x02,
        SPRINTING = 0x04
    }

    [Flags]
    public enum VehicleFlags : byte
    {
        NONE = 0x00,
        SIREN_ON = 0x01,
        DEAD = 0x02,
        LIGHTS = 0x04,
        SEARCH_LIGHTS = 0x08,
        INTERIOR_LIGHTS = 0x10,
        TAXI_LIGHTS = 0x20

    }
}
