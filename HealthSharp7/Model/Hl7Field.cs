namespace healthsharp7.Model
{
    public class Hl7Field
    {
        internal Hl7Field(string value)
        {
            Value = value;
        }

        private string Value { get; }

        public override string ToString()
        {
            return Value;
        }

        public static Hl7Field Parse(string field)
        {
            return new Hl7Field(field);
        }
    }
}