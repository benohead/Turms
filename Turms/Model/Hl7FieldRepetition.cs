namespace Turms.Model
{
    public class Hl7FieldRepetition: Hl7Element
    {
        private Hl7FieldRepetition(string value, Hl7Encoding encoding)
        {
            Value = value;
            Encoding = encoding;
        }

        protected override void FullyParse()
        {
            IsParsed = true;
        }

        public static Hl7FieldRepetition Parse(string repetition, Hl7Encoding encoding)
        {
            return new Hl7FieldRepetition(repetition, encoding);
        }

        public override string ToString()
        {
            EnsureFullyParsed();
            return Encoding.Unescape(Value);
        }
    }
}