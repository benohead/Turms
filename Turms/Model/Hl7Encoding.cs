namespace Turms.Model
{
    public class Hl7Encoding
    {
        public Hl7Encoding()
        {
            SegmentSeparator = new[] {"\r\n", "\n\r", "\r", "\n"};
            FieldSeparator = '|';
            ComponentSeparator = '^';
        }

        public string[] SegmentSeparator { get; set; }
        public char FieldSeparator { get; set; }
        public char ComponentSeparator { get; set; }
    }
}