using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class SetModelPacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 4)
            {
                return false;
            }
            ClientConnectionScript.CharacterModel = new Model(BitConverter.ToInt32(data, 0));
            ClientConnectionScript.Character.Delete();
            ClientConnectionScript.SpawnCharacter();
            return true;
        }
    }
}
