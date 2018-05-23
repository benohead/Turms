using System.Runtime.CompilerServices;
using HealthSharp7.Model;

[assembly: InternalsVisibleTo("HealthSharp7.UnitTest")]

namespace healthsharp7.Model
{
    public abstract class Hl7Element
    {
        protected internal Hl7Encoding Encoding { get; set; }
        internal bool IsParsed { get; set; }
        protected string Value { get; set; }

        protected void EnsureFullyParsed()
        {
            if (!IsParsed)
                FullyParse();
        }

        protected abstract void FullyParse();
    }
}