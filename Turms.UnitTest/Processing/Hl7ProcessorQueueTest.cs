using NSubstitute;
using Turms.Model;
using Turms.Processing;
using Xunit;

namespace Turms.UnitTest.Processing
{
    public class Hl7ProcessorQueueTest
    {
        private const string AdtA01Message =
                "MSH|^~\\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613083617||ADT^A01|934576120110613083617|P|2.3||||\r\nEVN|A01|20110613083617|||\r\nPID|1||135769||MOUSE^MICKEY^||19281118|M|||123 Main St.^^Lake Buena Vista^FL^32830||(407)939-1289^^^theMainMouse@disney.com|||||1719|99999999||||||||||||||||||||\r\nPV1|1|O|||||^^^^^^^^|^^^^^^^^"
            ;

        [Fact]
        public void ShouldTriggerAppropriateProcessor()
        {
            // Arrange
            var hl7ProcessorQueue = new Hl7ProcessorQueue();

            var processor1 = Substitute.For<IHl7Processor>();
            processor1.CanProcess(Arg.Any<Hl7Message>()).Returns(c => c.Arg<Hl7Message>().MessageType == "ADT^A01");
            hl7ProcessorQueue.AddProcessor(processor1);

            var processor2 = Substitute.For<IHl7Processor>();
            processor2.CanProcess(Arg.Any<Hl7Message>()).Returns(c => c.Arg<Hl7Message>().MessageType == "ADT^A02");
            hl7ProcessorQueue.AddProcessor(processor2);

            var processor3 = Substitute.For<IHl7Processor>();
            processor3.CanProcess(Arg.Any<Hl7Message>()).Returns(c => c.Arg<Hl7Message>().MessageType == "ADT^A03");
            hl7ProcessorQueue.AddProcessor(processor3);

            // Act
            hl7ProcessorQueue.Process(AdtA01Message);

            // Assert
            processor1.Received().ProcessMessage(Arg.Any<Hl7Message>());
            processor2.DidNotReceive().ProcessMessage(Arg.Any<Hl7Message>());
            processor3.DidNotReceive().ProcessMessage(Arg.Any<Hl7Message>());
        }
    }
}