using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAVRemultiplied.ServerSystem.PacketsIn
{
    public class RequestModelPacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(GTAVServerClientConnection client, byte[] data)
        {
            if (data.Length != 4)
            {
                return false;
            }
            client.CharacterModel = new Model(BitConverter.ToInt32(data, 0));
            client.Character.Delete();
            client.SpawnCharacter();
            return true;
        }
    }
}
