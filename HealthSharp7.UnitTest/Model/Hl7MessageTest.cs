using healthsharp7.Model;
using HealthSharp7.Model;
using Xunit;

namespace healthsharp7.UnitTest.Model
{
    public class Hl7MessageTest
    {
        private const string AdtA01Message =
            @"MSH|^~\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613083617||ADT^A01|934576120110613083617|P|2.3||||
EVN|A01|20110613083617|||
PID|1||135769||MOUSE^MICKEY^||19281118|M|||123 Main St.^^Lake Buena Vista^FL^32830||(407)939-1289^^^theMainMouse@disney.com|||||1719|99999999||||||||||||||||||||
PV1|1|O|||||^^^^^^^^|^^^^^^^^";

        private const string AdtA01MessageNoTrailingPipes =
            @"MSH|^~\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613083617||ADT^A01|934576120110613083617|P|2.3
EVN|A01|20110613083617
PID|1||135769||MOUSE^MICKEY^||19281118|M|||123 Main St.^^Lake Buena Vista^FL^32830||(407)939-1289^^^theMainMouse@disney.com|||||1719|99999999
PV1|1|O|||||^^^^^^^^|^^^^^^^^";

        private const string AdtA01MessageDollarAsSegmentSeparator =
                @"MSH|^~\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613083617||ADT^A01|934576120110613083617|P|2.3$EVN|A01|20110613083617$PID|1||135769||MOUSE^MICKEY^||19281118|M|||123 Main St.^^Lake Buena Vista^FL^32830||(407)939-1289^^^theMainMouse@disney.com|||||1719|99999999$PV1|1|O|||||^^^^^^^^|^^^^^^^^"
            ;

        [Fact]
        public void GetSegmentsShouldReturnExpectedSegmentCount()
        {
            // Arrange
            var message = Hl7Message.Parse(AdtA01Message);

            // Act
            var segments = message.Segments;

            // Assert
            Assert.Equal(4, segments.Count);
        }

        [Fact]
        public void GetSegmentsShouldReturnExpectedSegmentTypes()
        {
            // Arrange
            var message = Hl7Message.Parse(AdtA01Message);

            // Act
            var segments = message.Segments;

            // Assert
            Assert.Equal("MSH", segments[0].Name);
            Assert.Equal("EVN", segments[1].Name);
            Assert.Equal("PID", segments[2].Name);
            Assert.Equal("PV1", segments[3].Name);
        }

        [Fact]
        public void ShouldBeParsingMessageWithOtherSegmentSeparator()
        {
            //Act
            var encoding = new Hl7Encoding {SegmentSeparator = new[] {"$"}};
            var message = Hl7Message.Parse(AdtA01MessageDollarAsSegmentSeparator, encoding);

            //Assert
            Assert.Equal(AdtA01MessageDollarAsSegmentSeparator, message.ToString());
        }

        [Fact]
        public void ShouldParseCreatedMessage()
        {
            //Arrange
            var message = new Hl7Message()
                          + "MSH|^~\\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613083617||ADT^A01|934576120110613083617|P|2.3"
                          + "EVN|A01|20110613083617|||"
                          + "PID|1||135769||MOUSE^MICKEY^||19281118|M|||123 Main St.^^Lake Buena Vista^FL^32830||(407)939-1289^^^theMainMouse@disney.com|||||1719|99999999"
                          + "PV1|1|O|||||^^^^^^^^|^^^^^^^^";

            //Assert
            Assert.Equal(AdtA01MessageNoTrailingPipes, message.ToString());
        }

        [Fact]
        public void ToStringReturnsOriginalStringWithoutTrailingPipes()
        {
            //Act
            var message = Hl7Message.Parse(AdtA01MessageNoTrailingPipes);

            //Assert
            Assert.Equal(AdtA01MessageNoTrailingPipes, message.ToString());
        }
    }
}