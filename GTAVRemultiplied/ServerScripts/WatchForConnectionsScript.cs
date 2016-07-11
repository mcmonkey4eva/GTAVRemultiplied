using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTAVRemultiplied;
using GTAVRemultiplied.ServerSystem;

class WatchForConnectionsScript : Script
{
    public WatchForConnectionsScript()
    {
        Tick += WatchForConnectionsScript_Tick;
    }

    private void WatchForConnectionsScript_Tick(object sender, EventArgs e)
    {
        try
        {
            if (GTAVFreneticServer.Enabled)
            {
                GTAVFreneticServer.Connections.CheckForConnections();
                foreach (GTAVServerClientConnection conn in GTAVFreneticServer.Connections.Connections)
                {
                    if (conn.Character != null && conn.Name != null)
                    {
                        ClientConnectionScript.Text3D(conn.Character.GetBoneCoord(Bone.IK_Head) + new Vector3(0, 0, 0.25f), conn.Name, System.Drawing.Color.White, 2f);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
        }
    }
}
