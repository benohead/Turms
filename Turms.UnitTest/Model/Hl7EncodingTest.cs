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

        [Fact]
        public void ShouldEscapeData()
        {
            // Arrange
            const string content = "\r\n";

            // Act
            var result = new Hl7Encoding().EscapeAllCharacters(content);

            // Assert
            const string expectedResult = @"\X0D0A\";
            Assert.Equal(expectedResult, result);
        }
    }
}