using System;
using TechTalk.SpecFlow;
using Turms.AcceptanceTest.DSL;
using Xunit;

namespace Turms.AcceptanceTest
{
    [Binding]
    public class MessageFixingSteps
    {
        private readonly MessageDsl messageDsl;

        public MessageFixingSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            messageDsl = new MessageDsl(scenarioContext);
        }

        [Given(@"I have a message with a line break in the middle of a segment")]
        public void GivenIHaveAMessageWithALineBreakInTheMiddleOfASegment()
        {
            string message = @"MSH|^~\&|DDTEK LAB|ELAB-1|DDTEK OE|BLDG14|200502150930||ORU^R01^ORU_R01|CTRL-9876|P|2.4
PID|||010-11-1111||Estherhaus^Eva^E^^^^L|Smith|19720520|F|||256 Sherwood Forest Dr.^^Baton Rouge^LA^70809||(225)334-5232|(225)752-1213||||AC010111111||76-B4335^LA^20070520
OBR|1|948642^DDTEK OE|917363^DDTEK LAB|1554-5^GLUCOSE|||200502150730|||||||||020-22-2222^Levin-Epstein^Anna^^^^MD^^Micro-Managed
Health Associates|||||||||F|||||||030-33-3333&Honeywell&Carson&&&&MD
OBX|1|SN|1554-5^GLUCOSE^^^POST 12H CFST:MCNC:PT:SER/PLAS:QN||^175|mg/dl|70_105|H|||F";
            messageDsl.SetMessageString(message);
        }

        [When(@"I fix the message")]
        public void WhenIFixTheMessage()
        {
            messageDsl.FixMessage();
        }

        [Then(@"the fixed message can be parsed")]
        public void ThenTheFixedMessageCanBeParsed()
        {
            messageDsl.ParseMessage();
        }

        [Then(@"the fixed message cannot be parsed")]
        public void ThenTheFixedMessageCannotBeParsed()
        {
            Assert.Throws<ArgumentException>(() => { messageDsl.ParseMessage(); });
        }
    }
}