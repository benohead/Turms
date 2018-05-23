using System;
using Turms.Model;
using Xunit;

namespace Turms.UnitTest.Model
{
    public class Hl7MessageTest
    {
        private const string AdtA01Message =
            "MSH|^~\\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613083617||ADT^A01|934576120110613083617|P|2.3||||\r\nEVN|A01|20110613083617|||\r\nPID|1||135769||MOUSE^MICKEY^||19281118|M|||123 Main St.^^Lake Buena Vista^FL^32830||(407)939-1289^^^theMainMouse@disney.com|||||1719|99999999||||||||||||||||||||\r\nPV1|1|O|||||^^^^^^^^|^^^^^^^^";

        private const string AdtA01MessageNoTrailingPipes =
            "MSH|^~\\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613083617||ADT^A01|934576120110613083617|P|2.3\r\nEVN|A01|20110613083617\r\nPID|1||135769||MOUSE^MICKEY^||19281118|M|||123 Main St.^^Lake Buena Vista^FL^32830||(407)939-1289^^^theMainMouse@disney.com|||||1719|99999999\r\nPV1|1|O|||||^^^^^^^^|^^^^^^^^";

        private const string AdtA01MessageDollarAsSegmentSeparator =
                "MSH|^~\\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613083617||ADT^A01|934576120110613083617|P|2.3$EVN|A01|20110613083617$PID|1||135769||MOUSE^MICKEY^||19281118|M|||123 Main St.^^Lake Buena Vista^FL^32830||(407)939-1289^^^theMainMouse@disney.com|||||1719|99999999$PV1|1|O|||||^^^^^^^^|^^^^^^^^"
            ;

        private const string MessageMshOnlyOtherFieldDelimiter =
                "MSH$^~\\&$SENDING_APPLICATION$SENDING_FACILITY$RECEIVING_APPLICATION$RECEIVING_FACILITY$20110613083617$$ADT^A01$934576120110613083617$P$2.3$$$$"
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
        public void MessageShouldStartWithAnMshSegment()
        {
            //Assert
            Assert.Throws<ArgumentException>(() => Hl7Message.Parse("MSG|1"));
        }

        [Fact]
        public void ShouldBeParsingMessageWithOtherSegmentSeparator()
        {
            //Arrange
            var encoding = new Hl7Encoding {SegmentSeparator = new[] {"$"}};
            var message = Hl7Message.Parse(AdtA01MessageDollarAsSegmentSeparator, encoding);

            //Act
            var messageString = message.ToString();

            //Assert
            Assert.Equal(AdtA01MessageDollarAsSegmentSeparator, messageString);
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

            //Act
            var messageString = message.ToString();

            //Assert
            Assert.Equal(AdtA01MessageNoTrailingPipes, messageString);
        }

        [Fact]
        public void ShouldReadDelimitersFromMessage()
        {
            //Act
            var message = Hl7Message.Parse(MessageMshOnlyOtherFieldDelimiter);

            //Assert
            Assert.Equal('$', message.Encoding.FieldSeparator);
            Assert.Equal("RECEIVING_APPLICATION", message["MSH.4"].ToString());
        }

        [Fact]
        public void ShouldReturnNullForNotFoundFields()
        {
            //Arrange
            var message = Hl7Message.Parse(AdtA01Message);

            //Act
            var field = message["OBX.1"];

            //Assert
            Assert.Null(field);
        }

        [Fact]
        public void ShouldReturnNullForNotFoundSegments()
        {
            //Arrange
            var message = Hl7Message.Parse(AdtA01Message);

            //Act
            var segment = message["OBX"];

            //Assert
            Assert.Null(segment);
        }

        [Fact]
        public void ShouldReturnTheField()
        {
            //Arrange
            var message = Hl7Message.Parse(AdtA01Message);

            //Act
            var field = message["EVN.1"].ToString();

            //Assert
            Assert.Equal("A01", field);
        }

        [Fact]
        public void ShouldReturnTheMessageType()
        {
            //Arrange
            var message = Hl7Message.Parse(AdtA01Message);

            //Act
            var controlId = message.MessageType;

            //Assert
            Assert.Equal("ADT^A01", controlId);
        }

        [Fact]
        public void ShouldReturnTheMessageControlId()
        {
            //Arrange
            var message = Hl7Message.Parse(AdtA01Message);

            //Act
            var controlId = message.MessageControlId;

            //Assert
            Assert.Equal("934576120110613083617", controlId);
        }

        [Fact]
        public void ShouldReturnTheProcessingId()
        {
            //Arrange
            var message = Hl7Message.Parse(AdtA01Message);

            //Act
            var processingId = message.ProcessingId;

            //Assert
            Assert.Equal("P", processingId);
        }

        [Fact]
        public void ShouldReturnTheMessageVersion()
        {
            //Arrange
            var message = Hl7Message.Parse(AdtA01Message);

            //Act
            var version = message.MessageVersion;

            //Assert
            Assert.Equal("2.3", version);
        }

        [Fact]
        public void ShouldReturnTheSegment()
        {
            //Arrange
            var message = Hl7Message.Parse(AdtA01Message);

            //Act
            var segment = message["EVN"].ToString();

            //Assert
            Assert.Equal("EVN|A01|20110613083617", segment);
        }

        [Fact]
        public void ToStringReturnsOriginalStringWithoutTrailingPipes()
        {
            //Arrange
            var message = Hl7Message.Parse(AdtA01MessageNoTrailingPipes);

            //Act
            var messageString = message.ToString();

            //Assert
            Assert.Equal(AdtA01MessageNoTrailingPipes, messageString);
        }
    }
}