using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAVRemultiplied.ClientSystem.PacketsIn
{
    public class AddPropPacketIn : AbstractPacketIn
    {
        public override bool ParseAndExecute(byte[] data)
        {
            if (data.Length != 4 + 4 + 12 + 4 + 1)
            {
                return false;
            }
            int id = BitConverter.ToInt32(data, 0);
            int hash = BitConverter.ToInt32(data, 4);
            Vector3 pos = new Vector3();
            pos.X = BitConverter.ToSingle(data, 4 + 4);
            pos.Y = BitConverter.ToSingle(data, 4 + 4 + 4);
            pos.Z = BitConverter.ToSingle(data, 4 + 4 + 8);
            float heading = BitConverter.ToSingle(data, 4 + 4 + 12);
            Prop prop = World.CreateProp(new Model(hash), pos, new Vector3(0, 0, heading), false, false);
            if (prop == null)
            {
                prop = World.CreateProp(new Model(hash), Game.Player.Character.Position + new Vector3(10, 10, 10), new Vector3(0, 0, heading), false, false);
                if (prop == null)
                {
                    Log.Message("Warning", "Null prop spawned: " + hash, 'Y');
                    return true;
                }
            }
            prop.IsPersistent = true;
            prop.IsInvincible = true;
            prop.FreezePosition = true; //(data[4 + 4 + 12 + 4] & 1) == 1;
            ClientConnectionScript.ServerToClientProp[id] = prop.Handle;
            ClientConnectionScript.ClientToServerProp[prop.Handle] = id;
            return true;
        }
    }
}
