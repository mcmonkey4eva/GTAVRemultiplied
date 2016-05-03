using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
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
            }
        }
        catch (Exception ex)
        {
            Log.Exception(ex);
        }
    }
}
