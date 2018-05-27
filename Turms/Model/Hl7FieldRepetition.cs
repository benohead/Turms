namespace Turms.Model
{
    public class Hl7FieldRepetition : Hl7Element
    {
        public Hl7FieldRepetition(string value, Hl7Encoding encoding)
        {
            Value = value;
            Encoding = encoding;
        }

        public Hl7FieldRepetition(string value) : this(value, new Hl7Encoding())
        {
        }

        public Hl7FieldRepetition() : this("")
        {
        }

        protected override void FullyParse()
        {
            IsParsed = true;
        }

        public override Hl7Element this[int i]
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
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