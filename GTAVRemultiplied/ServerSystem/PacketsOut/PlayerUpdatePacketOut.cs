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
        public PlayerUpdatePacketOut(Player character) // TODO: GTAV-RMP Player object rather than GTA Player?
        {
            // TODO: Player ID!
            ID = ServerToClientPacket.PLAYER_UPDATE;
            Data = new byte[16 + 12 + 4 + 12];
            BitConverter.GetBytes(character.Character.Position.X).CopyTo(Data, 0);
            BitConverter.GetBytes(character.Character.Position.Y).CopyTo(Data, 4);
            BitConverter.GetBytes(character.Character.Position.Z).CopyTo(Data, 8);
            BitConverter.GetBytes(character.Character.Heading).CopyTo(Data, 12);
            Vector3 aim = Vector3.Zero;
            if (Game.Player.IsAiming)
            {
                aim = GameplayCamera.Direction; // TODO: Player object camera
            }
            BitConverter.GetBytes(aim.X).CopyTo(Data, 16);
            BitConverter.GetBytes(aim.Y).CopyTo(Data, 16 + 4);
            BitConverter.GetBytes(aim.Z).CopyTo(Data, 16 + 8);
            BitConverter.GetBytes((uint)character.Character.Weapons.Current.Hash).CopyTo(Data, 16 + 12);
            BitConverter.GetBytes(character.Character.Velocity.X).CopyTo(Data, 16 + 12 + 4);
            BitConverter.GetBytes(character.Character.Velocity.Y).CopyTo(Data, 16 + 12 + 4 + 4);
            BitConverter.GetBytes(character.Character.Velocity.Z).CopyTo(Data, 16 + 12 + 4 + 8);
        }
    }
}
