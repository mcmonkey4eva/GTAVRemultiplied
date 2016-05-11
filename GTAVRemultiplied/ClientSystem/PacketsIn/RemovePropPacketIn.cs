using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class RemovePropPacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 4)
            {
                return false;
            }
            int id = BitConverter.ToInt32(data, 0);
            int pr;
            if (ClientConnectionScript.ServerToClientProp.TryGetValue(id, out pr))
            {
                Prop prop = new Prop(pr);
                prop.Delete();
                ClientConnectionScript.ServerToClientProp.Remove(id);
                ClientConnectionScript.ClientToServerProp.Remove(pr);
            }
            else
            {
                Log.Message("Warning", "Unknown vehicle removed!", 'Y');
            }
            return true;
        }
    }
}
