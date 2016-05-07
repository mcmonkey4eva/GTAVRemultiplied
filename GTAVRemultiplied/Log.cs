using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

        public static void Error(string message)
        {
            Message("Error", "INTERNAL ERROR: " + message, 'R');
            File.AppendAllText(Environment.CurrentDirectory + "/frenetic/" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + ".log", "[[ERROR]]: " + message + Environment.NewLine);
        }

        public static void Exception(Exception ex)
        {
            Error(ex.ToString());
        }
    }
}
