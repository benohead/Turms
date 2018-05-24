using System;
using Turms.Model;
using Xunit;

namespace Turms.UnitTest.Model
{
    public class Hl7FieldTest
    {
        [Fact]
        public void ParsingWithANullEncodingShouldThrowAnArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Hl7Field.Parse("1", null));
        }
    }
}