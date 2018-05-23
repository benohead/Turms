using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Turms.UnitTest")]

namespace Turms.Model
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