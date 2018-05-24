using TechTalk.SpecFlow;
using Turms.BDD.DSL;

namespace Turms.BDD
{
    [Binding]
    public class MessageParsingSteps
    {
        private const string AdtA01Message =
            "MSH|^~\\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613083617||ADT^A01|934576120110613083617|P|2.3||||\r\nEVN|A01|20110613083617|||\r\nPID|1||135769||MOUSE^MICKEY^||19281118|M|||123 Main St.^^Lake Buena Vista^FL^32830||(407)939-1289^^^theMainMouse@disney.com|||||1719|99999999||||||||||||||||||||\r\nPV1|1|O|||||^^^^^^^^|^^^^^^^^";

        private MessageDsl messageDsl;

        [BeforeScenario]
        public void Initialize()
        {
            messageDsl = new MessageDsl();
        }

        [Given(@"I have an ADT\^A(.*) message")]
        public void GivenIHaveAnADTAMessage(int p0)
        {
            switch (p0)
            {
                case 1:
                    MessageDsl.SetMessageString(AdtA01Message);
                    break;
            }
        }
        
        [When(@"I parse the message")]
        public void WhenIParseTheMessage()
        {
            MessageDsl.ParseMessage();
        }
        
        [Then(@"the patient ID can be extracted")]
        public void ThenThePatientIDCanBeExtracted()
        {
            MessageDsl.ConfirmPatientId("135769");
        }
        
        [Then(@"the patient name can be extracted")]
        public void ThenThePatientNameCanBeExtracted()
        {
            MessageDsl.ConfirmPatientName("MOUSE^MICKEY");
        }
    }
}
