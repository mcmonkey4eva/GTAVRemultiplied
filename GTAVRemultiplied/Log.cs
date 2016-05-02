using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVRemultiplied
{
    public class Log
    {
        public static void Message(string type, string text, char color = 'w')
        {
            ChatTextScript.Instance.AddLine(color, type, text);
        }

        public static void Exception(Exception ex)
        {
            Message("Exception", "INTERNAL ERROR: " + ex.ToString(), 'R');
        }
    }
}
