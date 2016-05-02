using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTAVRemultiplied
{
    /// <summary>
    /// TODO: Irrelevantize/replace this!
    /// </summary>
    public class CharacterUtilities
    {
        public static char GetCharFrom(Keys key, bool shift)
        {
            if (key >= Keys.A && key <= Keys.Z)
            {
                return shift ? (char)(key - Keys.A + 'A') : (char)(key - Keys.A + 'a');
            }
            switch (key)
            {
                case Keys.D0:
                    return shift ? ')' : '0';
                case Keys.D1:
                    return shift ? '!' : '1';
                case Keys.D2:
                    return shift ? '@' : '2';
                case Keys.D3:
                    return shift ? '#' : '3';
                case Keys.D4:
                    return shift ? '$' : '4';
                case Keys.D5:
                    return shift ? '%' : '5';
                case Keys.D6:
                    return shift ? '^' : '6';
                case Keys.D7:
                    return shift ? '&' : '7';
                case Keys.D8:
                    return shift ? '*' : '8';
                case Keys.D9:
                    return shift ? '(' : '9';
                case Keys.Space:
                    return ' ';
                case Keys.Tab:
                    return '\t';
                case Keys.OemPeriod:
                    return shift ? '>' : '.';
                case Keys.OemSemicolon:
                    return shift ? ':' : ';';
                case Keys.Oemcomma:
                    return shift ? '<' : ',';
                case Keys.OemOpenBrackets:
                    return shift ? '{' : '[';
                case Keys.OemCloseBrackets:
                    return shift ? '}' : ']';
                case Keys.OemQuotes:
                    return shift ? '\"' : '\'';
                case Keys.OemQuestion:
                    return shift ? '?' : '/';
                case Keys.OemBackslash:
                    return shift ? '|' : '\\';
                case Keys.OemMinus:
                    return shift ? '_' : '-';
                case Keys.OemPipe:
                    return shift ? '|' : '\\';
                case Keys.Oemplus:
                    return shift ? '+' : '=';
                case Keys.Oemtilde:
                    return shift ? '~' : '`';
                default:
                    return '\0';
            }
        }
    }
}
