namespace HealthSharp7.Model
{
    public class Hl7Encoding
    {
        public string[] SegmentSeparator { get; set; }
        public char FieldSeparator { get; set; }

        public Hl7Encoding()
        {
            SegmentSeparator = new[] {"\r\n", "\n\r", "\r", "\n"};
            FieldSeparator = '|';
        }
    }
}