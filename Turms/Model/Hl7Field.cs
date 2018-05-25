using System;

namespace Turms.Model
{
    public class Hl7Field : Hl7Element
    {
        public Hl7Field(string value) : this(value, new Hl7Encoding())
        {
        }

        private Hl7Field(string value, Hl7Encoding encoding)
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            Value = value.TrimEnd(encoding.ComponentSeparator);
            Encoding = encoding;
        }

        public override string ToString()
        {
            EnsureFullyParsed();
            return Encoding.Unescape(Value);
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