namespace healthsharp7.Model
{
    public class Hl7Field
    {
        private string Value { get; }

        internal Hl7Field(string value)
        {
            Value = value;
        }

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