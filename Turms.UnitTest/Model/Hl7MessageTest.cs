using System;
using Turms.Model;
using Xunit;

namespace Turms.UnitTest.Model
{
    public class Hl7MessageTest
    {
        private const string AdtA01Message =
            "MSH|^~\\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613083617||ADT^A01|934576120110613083617|P|2.3||||\r\nEVN|A01|20110613083617|||\r\nPID|1||135769||MOUSE^MICKEY^||19281118|M|||123 Main St.^^Lake Buena Vista^FL^32830||(407)939-1289^^^theMainMouse@disney.com|||||1719|99999999||||||||||||||||||||\r\nPV1|1|O|||||^^^^^^^^|^^^^^^^^";

        private const string AdtA01MessageNoTrailingSeparators =
            "MSH|^~\\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613083617||ADT^A01|934576120110613083617|P|2.3\r\nEVN|A01|20110613083617\r\nPID|1||135769||MOUSE^MICKEY||19281118|M|||123 Main St.^^Lake Buena Vista^FL^32830||(407)939-1289^^^theMainMouse@disney.com|||||1719|99999999\r\nPV1|1|O";

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
            var message = Hl7Message.Parse(AdtA01Message.Replace("\r\n", "$"), encoding);

            //Act
            var messageString = message.ToString();

            //Assert
            Assert.Equal(AdtA01MessageNoTrailingSeparators.Replace("\r\n", "$"), messageString);
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
            Assert.Equal(AdtA01MessageNoTrailingSeparators, messageString);
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
            var message = Hl7Message.Parse(AdtA01MessageNoTrailingSeparators);

            //Act
            var messageString = message.ToString();

            //Assert
            Assert.Equal(AdtA01MessageNoTrailingSeparators, messageString);
        }

        [Fact]
        public void ShouldUnescapeData()
        {
            // Arrange
            const string content = @"MSH|^~\&|TestSys|432^testsys practice|TEST||201402171537||MDM^T02|121906|P|2.3.1||||||||
OBX|1|TX|PROBLEM FOCUSED^PROBLEM FOCUSED^test|1|\T\#39;Thirty days have September,\X000d\April\X0A\June,\X0A\and November.\X0A\When short February is done,\E\X0A\E\all the rest have\T\nbsp;31.\T\#39";

            var msg = Hl7Message.Parse(content);


            // Act
            var obx5 = msg["OBX.5"].ToString();

            // Assert
            const string expectedResult =
                "&#39;Thirty days have September,\rApril\nJune,\nand November.\nWhen short February is done,\\X0A\\all the rest have&nbsp;31.&#39";
            Assert.Equal(expectedResult, obx5);
        }

        [Fact]
        public void ShouldThrowExceptionWhenThereAreTooManyParsingDelimiters()
        {
            //Arrange
            const string message = @"MSH|^~\ &|LIFTLAB||UBERMED||201701131234||ORU^R01|K113|P|";

            //Assert
            Assert.Throws<ArgumentException>(() => { Hl7Message.Parse(message); });
        }

        [Fact]
        public void ShouldThrowExceptionWhenParsingDelimitersAreMissing()
        {
            //Arrange
            const string message = @"MSH||LIFTLAB||UBERMED||201701131234||ORU^R01|K113|P|";

            //Assert
            Assert.Throws<ArgumentException>(() => { Hl7Message.Parse(message); });
        }

        [Fact]
        public void ShouldThrowExceptionWhenParsingDelimitersAreAlphanumeric()
        {
            //Arrange
            const string message = @"MSH|AB12|LIFTLAB||UBERMED||201701131234||ORU^R01|K113|P|";

            //Assert
            Assert.Throws<ArgumentException>(() => { Hl7Message.Parse(message); });
        }

        [Fact]
        public void ShouldThrowExceptionWhenParsingDelimitersAreIdentical()
        {
            //Arrange
            const string message1 = @"MSH|^~|&|LIFTLAB||UBERMED||201701131234||ORU^R01|K113|P|";
            const string message2 = @"MSH|^~^&|LIFTLAB||UBERMED||201701131234||ORU^R01|K113|P|";
            const string message3 = @"MSH|^~~&|LIFTLAB||UBERMED||201701131234||ORU^R01|K113|P|";
            const string message4 = @"MSH|^&\&|LIFTLAB||UBERMED||201701131234||ORU^R01|K113|P|";

            //Assert
            Assert.Throws<ArgumentException>(() => { Hl7Message.Parse(message1); });
            Assert.Throws<ArgumentException>(() => { Hl7Message.Parse(message2); });
            Assert.Throws<ArgumentException>(() => { Hl7Message.Parse(message3); });
            Assert.Throws<ArgumentException>(() => { Hl7Message.Parse(message4); });
        }
    }
}