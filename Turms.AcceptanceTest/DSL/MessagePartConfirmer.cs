using Turms.Model;
using Xunit;

namespace Turms.AcceptanceTest.DSL
{
    internal class MessagePartConfirmer
    {
        private readonly Hl7Element hl7Element;

        public MessagePartConfirmer(Hl7Element hl7Element)
        {
            this.hl7Element = hl7Element;
        }

        public void Equal(string expected)
        {
            Assert.Equal(expected, hl7Element.ToString());
        }
    }
}