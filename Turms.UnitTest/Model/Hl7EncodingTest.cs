using Turms.Model;
using Xunit;

namespace Turms.UnitTest.Model
{
    public class Hl7EncodingTest
    {
        [Fact]
        public void ShouldUnescapeData()
        {
            // Arrange
            const string content = @"\T\\E\\H\\N\\F\\R\\S\\X0A\";

            // Act
            var result = new Hl7Encoding().Unescape(content);

            // Assert
            const string expectedResult = "&\\H\\\\N\\|~^\n";
            Assert.Equal(expectedResult, result);
        }
    }
}