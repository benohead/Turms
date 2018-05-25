using System;
using System.Collections;
using System.Text;

namespace Turms.Model
{
    public class Hl7Encoding
    {
        public Hl7Encoding()
        {
            SegmentSeparator = new[] {"\r\n", "\n\r", "\r", "\n"};
            FieldSeparator = '|';
            ComponentSeparator = '^';
            EscapeCharacter = '\\';
            SubcomponentSeparator = '&';
            RepetitionSeparator = '~';
        }

        public string[] SegmentSeparator { get; set; }
        public char FieldSeparator { get; set; }
        public char ComponentSeparator { get; set; }
        public char EscapeCharacter { get; set; }
        public char SubcomponentSeparator { get; set; }
        public char RepetitionSeparator { get; set; }

        public string Unescape(string value)
        {
            if (value.IndexOf(EscapeCharacter) == -1)
            {
                return value;
            }

            StringBuilder result = new StringBuilder();
            int textLength = value.Length;
            var previousEscape = 0;
            var nextEscape = value.IndexOf(EscapeCharacter, previousEscape);
            while (nextEscape != -1 && (nextEscape + 1) < textLength)
            {
                if (nextEscape > previousEscape)
                {
                    result.Append(value.Substring(previousEscape, nextEscape - previousEscape));
                }
                previousEscape = nextEscape + 1;
                switch (value[nextEscape + 1])
                {
                    case 'C':
                    //Single-byte character set escape sequence with two hexadecimal values not converted
                    case 'M':
                    //Multi-byte character set escape sequence with two or three hexadecimal values (zz is optional) not converted
                    case 'Z':
                    //Locally defined escape sequence not converted
                    case 'H':
                    //Start highlighting not converted
                    case 'N':
                        //Normal text (end highlighting) not converted
                        nextEscape = value.IndexOf(EscapeCharacter, previousEscape + 1);
                        nextEscape = value.IndexOf(EscapeCharacter, nextEscape + 1);
                        break;
                    case 'E':
                        //Escape character converted to escape character (e.g., ‘\’)
                        result.Append(EscapeCharacter);
                        nextEscape += 2;
                        previousEscape = nextEscape + 1;
                        break;
                    case 'F':
                        //Field separator converted to field separator character (e.g., ‘|’)
                        result.Append(FieldSeparator);
                        nextEscape += 2;
                        previousEscape = nextEscape + 1;
                        break;
                    case 'R':
                        //Repetition separator converted to repetition separator character (e.g., ‘~’)
                        result.Append(RepetitionSeparator);
                        nextEscape += 2;
                        previousEscape = nextEscape + 1;
                        break;
                    case 'S':
                        //Component separator converted to component separator character (e.g., ‘^’)
                        result.Append(ComponentSeparator);
                        nextEscape += 2;
                        previousEscape = nextEscape + 1;
                        break;
                    case 'T':
                        //Subcomponent separator converted to subcomponent separator character (e.g., ‘&’)
                        result.Append(SubcomponentSeparator);
                        nextEscape += 2;
                        previousEscape = nextEscape + 1;
                        break;
                    case 'X':
                        nextEscape = value.IndexOf(EscapeCharacter, nextEscape + 1);
                        var s = value.Substring(previousEscape +1, nextEscape - previousEscape - 1);
                        result.Append(FromHexString(s));
                        previousEscape = nextEscape + 1;
                        break;
                    default:
                        previousEscape--;
                        break;
                }
                nextEscape = value.IndexOf(EscapeCharacter, nextEscape + 1);
            }
            if (nextEscape == -1)
            {
                result.Append(value.Substring(previousEscape));
            }
            return result.ToString();
        }

        //public static string ToHexString(string str)
        //{
        //    var sb = new StringBuilder();

        //    var bytes = Encoding.Unicode.GetBytes(str);
        //    foreach (var t in bytes)
        //    {
        //        sb.Append(t.ToString("X2"));
        //    }

        //    return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        //}

        public static string FromHexString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.ASCII.GetString(bytes).Replace("" + (char)0, "");
        }
    }
}