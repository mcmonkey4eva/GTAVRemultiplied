using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using FreneticScript;

namespace GTAVRemultiplied.ServerSystem.PacketsOut
{
    public class PlayerUpdatePacketOut : AbstractPacketOut
    {
        public PlayerUpdatePacketOut(Ped character, Vector3 aim)
        {
            // TODO: Player ID!
            ID = ServerToClientPacket.PLAYER_UPDATE;
            Data = new byte[16 + 12 + 4 + 12 + 4];
            BitConverter.GetBytes(character.Position.X).CopyTo(Data, 0);
            BitConverter.GetBytes(character.Position.Y).CopyTo(Data, 4);
            BitConverter.GetBytes(character.Position.Z).CopyTo(Data, 8);
            BitConverter.GetBytes(character.Heading).CopyTo(Data, 12);
            BitConverter.GetBytes(aim.X).CopyTo(Data, 16);
            BitConverter.GetBytes(aim.Y).CopyTo(Data, 16 + 4);
            BitConverter.GetBytes(aim.Z).CopyTo(Data, 16 + 8);
            BitConverter.GetBytes((uint)character.Weapons.Current.Hash).CopyTo(Data, 16 + 12);
            BitConverter.GetBytes(character.Velocity.X).CopyTo(Data, 16 + 12 + 4);
            BitConverter.GetBytes(character.Velocity.Y).CopyTo(Data, 16 + 12 + 4 + 4);
            BitConverter.GetBytes(character.Velocity.Z).CopyTo(Data, 16 + 12 + 4 + 8);
            BitConverter.GetBytes(character.Handle).CopyTo(Data, 16 + 12 + 4 + 12);
        }
    }
}
