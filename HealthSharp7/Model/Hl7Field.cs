namespace healthsharp7.Model
{
    public class Hl7Field: Hl7Element
    {
        internal Hl7Field(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            EnsureFullyParsed();
            return Value;
        }

        public static Hl7Field Parse(string field)
        {
            return new Hl7Field(field);
        }

        protected override void FullyParse()
        {
            IsParsed = true;
        }
    }
}