namespace Turms.Model
{
    public class Hl7Component : Hl7Element
    {
        private Hl7Component(string value, Hl7Encoding encoding)
        {
            Encoding = encoding;
            Value = value;
        }

        public override Hl7Element this[int i] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public static Hl7Component Parse(string component, Hl7Encoding encoding)
        {
            return new Hl7Component(component, encoding);
        }

        protected override void FullyParse()
        {
            IsParsed = true;
        }

        public override string ToString()
        {
            EnsureFullyParsed();
            return Encoding.Unescape(Value);
        }

        public string ToEscapedString()
        {
            EnsureFullyParsed();
            return Value;
        }

    }
}