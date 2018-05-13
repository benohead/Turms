using HealthSharp7.Model;

namespace healthsharp7.Model
{
    public class Hl7Field: Hl7Element
    {
        public Hl7Field(string value): this(value, new Hl7Encoding())
        {
        }

        internal Hl7Field(string value, Hl7Encoding encoding)
        {
            Value = value;
            Encoding = encoding;
        }

        public override string ToString()
        {
            EnsureFullyParsed();
            return Value;
        }

        public static Hl7Field Parse(string field, Hl7Encoding encoding)
        {
            return new Hl7Field(field, encoding);
        }

        protected override void FullyParse()
        {
            IsParsed = true;
        }
    }
}