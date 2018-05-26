using System;
using Turms.Model;
using Xunit;

namespace Turms.UnitTest.Model
{
    public class Hl7MessageTest
    {
        private const string AdtA01Message =
                "MSH|^~\\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613083617||ADT^A01|934576120110613083617|P|2.3||||\r\nEVN|A01|20110613083617|||\r\nPID|1||135769||MOUSE^MICKEY^||19281118|M|||123 Main St.^^Lake Buena Vista^FL^32830||(407)939-1289^^^theMainMouse@disney.com|||||1719|99999999||||||||||||||||||||\r\nPV1|1|O|||||^^^^^^^^|^^^^^^^^"
            ;

        private const string AdtA01MessageNoTrailingSeparators =
                "MSH|^~\\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613083617||ADT^A01|934576120110613083617|P|2.3\r\nEVN|A01|20110613083617\r\nPID|1||135769||MOUSE^MICKEY||19281118|M|||123 Main St.^^Lake Buena Vista^FL^32830||(407)939-1289^^^theMainMouse@disney.com|||||1719|99999999\r\nPV1|1|O"
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
            const string content =
                @"MSH|^~\&|TestSys|432^testsys practice|TEST||201402171537||MDM^T02|121906|P|2.3.1||||||||
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

        [Fact]
        public void ShouldBeAbleToHandleLeadingWhitespaceOnMSH()
        {
            //Arrange
            const string message =
                @"    MSH|^~\&|MACHETELAB|^DOSC|MACHETE|18779|20130405125146269||ORM^O01|1999077678|P|2.3|||AL|AL
PID|1|000000000026|60043^^^MACHETE^MRN||MACHETE^JOE||19890909|F|||123 SEASAME STREET^^Oakland^CA^94600||5101234567|5101234567||||||||||||||||N
ORC|NW|PRO2350||XO934N|||^^^^^R||20130405125144|91238^Machete^Joe||92383^Machete^Janice
OBR|1|PRO2350||11636^Urinalysis, with Culture if Indicated^L|||20130405135133||||N|||||92383^Machete^Janice|||||||||||^^^^^R";

            //Act
            var hl7Message = Hl7Message.Parse(message);

            //Assert
            Assert.Equal("MSH", hl7Message["MSH.0"].ToString());
        }

        [Fact]
        public void ShouldFixInvalidNewLinesInMessage()
        {
            //Arrange
            string message = @"MSH|^~\&|DDTEK LAB|ELAB-1|DDTEK OE|BLDG14|200502150930||ORU^R01^ORU_R01|CTRL-9876|P|2.4
PID|||010-11-1111||Estherhaus^Eva^E^^^^L|Smith|19720520|F|||256 Sherwood Forest Dr.^^Baton Rouge^LA^70809||(225)334-5232|(225)752-1213||||AC010111111||76-B4335^LA^20070520
OBR|1|948642^DDTEK OE|917363^DDTEK LAB|1554-5^GLUCOSE|||200502150730|||||||||020-22-2222^Levin-Epstein^Anna^^^^MD^^Micro-Managed
Health Associates|||||||||F|||||||030-33-3333&Honeywell&Carson&&&&MD
OBX|1|SN|1554-5^GLUCOSE^^^POST 12H CFST:MCNC:PT:SER/PLAS:QN||^175|mg/dl|70_105|H|||F";

            //Act
            var fixedMessage = Hl7Message.Fix(message);

            //Assert
            Assert.Throws<ArgumentException>(() => Hl7Message.Parse(message).EnsureFullyParsed());
            Hl7Message.Parse(fixedMessage).EnsureFullyParsed();
        }

        [Fact]
        public void ShouldNotFixEmptySegments()
        {
            //Arrange
            string message = "MSH|^~\\&|DDTEK LAB|ELAB-1|DDTEK OE|BLDG14|200502150930||ORU^R01^ORU_R01|CTRL-9876|P|2.4\r\nPID|||010-11-1111||Estherhaus^Eva^E^^^^L|Smith|19720520|F|||256 Sherwood Forest Dr.^^Baton Rouge^LA^70809||(225)334-5232|(225)752-1213||||AC010111111||76-B4335^LA^20070520\r\nOBR|1|948642^DDTEK OE|917363^DDTEK LAB|1554-5^GLUCOSE|||200502150730|||||||||020-22-2222^Levin-Epstein^Anna^^^^MD^^Micro-Managed\r\nOBX";

            //Act
            var fixedMessage = Hl7Message.Fix(message);

            //Assert
            Assert.Equal(message, fixedMessage);
        }
    }
}