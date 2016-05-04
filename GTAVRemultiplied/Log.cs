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
            foreach (string str in text.Replace("\r", "").Split('\n'))
            {
                ChatTextScript.Instance.AddLine(color, type, str);
            }
        }

        public static void Exception(Exception ex)
        {
            Message("Exception", "INTERNAL ERROR: " + ex.ToString(), 'R');
        }
    }
}
