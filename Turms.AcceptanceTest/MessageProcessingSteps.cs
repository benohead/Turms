using System;
using TechTalk.SpecFlow;
using Turms.AcceptanceTest.DSL;

namespace Turms.AcceptanceTest
{
    [Binding]
    public class MessageProcessingSteps
    {
        private readonly ProcessorDsl processorDsl;

        private const string AdtA01Message =
                "MSH|^~\\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613083617||ADT^A01|934576120110613083617|P|2.3||||\r\nEVN|A01|20110613083617|||\r\nPID|1||135769||MOUSE^MICKEY^||19281118|M|||123 Main St.^^Lake Buena Vista^FL^32830||(407)939-1289^^^theMainMouse@disney.com|||||1719|99999999||||||||||||||||||||\r\nPV1|1|O|||||^^^^^^^^|^^^^^^^^"
            ;

        public MessageProcessingSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            processorDsl = new ProcessorDsl(scenarioContext);
        }

        [Given(@"I have an ADT\^A(.*) message processor")]
        public void GivenIHaveAnADTAMessageProcessor(int p0)
        {
            processorDsl.RegisterProcessor($"ADT^A{p0:D2}");
        }

        [When(@"I process an ADT\^A(.*) message")]
        public void WhenIProcessAnADTAMessage(int p0)
        {
            switch (p0)
            {
                case 1:
                    processorDsl.ProcessMessage(AdtA01Message);
                    break;
            }
        }

        [Then(@"the ADT\^A(.*) message processor is triggered")]
        public void ThenTheADTAMessageProcessorIsTriggered(int p0)
        {
            processorDsl.ConfirmProcessorTriggered($"ADT^A{p0:D2}");
        }

        [Then(@"the ADT\^A(.*) message processor is not triggered")]
        public void ThenTheADTAMessageProcessorIsNotTriggered(int p0)
        {
            processorDsl.ConfirmProcessorNotTriggered($"ADT^A{p0:D2}");
        }
    }
}