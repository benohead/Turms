using System;
using TechTalk.SpecFlow;
using Turms.AcceptanceTest.DSL;

namespace Turms.AcceptanceTest
{
    [Binding]
    public class MessageParsingSteps
    {
        private readonly MessageDsl messageDsl;

        private const string AdtA01Message =
                "MSH|^~\\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613083617||ADT^A01|934576120110613083617|P|2.3||||\r\nEVN|A01|20110613083617|||\r\nPID|1||135769||MOUSE^MICKEY^||19281118|M|||123 Main St.^^Lake Buena Vista^FL^32830||(407)939-1289^^^theMainMouse@disney.com|||||1719|99999999||||||||||||||||||||\r\nPV1|1|O|||||^^^^^^^^|^^^^^^^^"
            ;
        private const string MessageWithEncodedCharacters = @"MSH|^~\&|TestSys|432^testsys practice|TEST||201402171537||MDM^T02|121906|P|2.3.1
OBX|1|TX|PROBLEM FOCUSED^PROBLEM FOCUSED^test|1|\T\#39;Thirty days have September,\X000d\April\X0A\June,\X0A\and November.\X0A\When short February is done,\E\X0A\E\all the rest have\T\nbsp;31.\T\#39";

        public MessageParsingSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            messageDsl = new MessageDsl(scenarioContext);
        }

        [Given(@"I have an ADT\^A(.*) message")]
        public void GivenIHaveAnADTAMessage(int p0)
        {
            switch (p0)
            {
                case 1:
                    messageDsl.SetMessageString(AdtA01Message);
                    break;
            }
        }

        [When(@"I parse the message")]
        public void WhenIParseTheMessage()
        {
            messageDsl.ParseMessage();
        }

        [Then(@"the patient ID can be extracted")]
        public void ThenThePatientIDCanBeExtracted()
        {
            messageDsl.ConfirmPatientId("135769");
        }

        [Then(@"the patient name can be extracted")]
        public void ThenThePatientNameCanBeExtracted()
        {
            messageDsl.ConfirmPatientName("MOUSE^MICKEY");
        }

        [Given(@"I have a message with encoded characters")]
        public void GivenIHaveAMessageWithEncodedCharacters()
        {
            messageDsl.SetMessageString(MessageWithEncodedCharacters);
        }

        [Then(@"the fields with encoded characters can be interpreted properly")]
        public void ThenTheFieldsWithEncodedCharactersCanBeInterpretedProperly()
        {
            messageDsl.ConfirmMessagePart("OBX.5").Equal("&#39;Thirty days have September,\rApril\nJune,\nand November.\nWhen short February is done,\\X0A\\all the rest have&nbsp;31.&#39");
        }

        [Then(@"the message serializes back to the original message")]
        public void ThenTheMessageSerializesBackToTheOriginalMessage()
        {
            messageDsl.ConfirmSerializedMessageIsIdentical();
        }

    }
}