using System;
using TechTalk.SpecFlow;
using Turms.AcceptanceTest.DSL;

namespace Turms.AcceptanceTest
{
    [Binding]
    public class MessageCreationSteps
    {
        private readonly MessageDsl messageDsl;

        public MessageCreationSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            messageDsl = new MessageDsl(scenarioContext);
        }

        [Given(@"I create a new message")]
        public void GivenICreateANewMessage()
        {
            messageDsl.CreateNewMessage();
            messageDsl.SetMessageType($"ADT^A01");
        }

        [Given(@"I populate the MSH Segment")]
        public void GivenIPopulateTheMSHSegment()
        {
            messageDsl.SetSendingApplication("TestSendingSystem");
            messageDsl.SetSequenceNumber("123");
        }

        [Given(@"I populate the PID Segment")]
        public void GivenIPopulateThePIDSegment()
        {
            messageDsl.AddSegment("PID");
            messageDsl.SetSurname("Doe");
            messageDsl.SetGivenName("John");
            messageDsl.SetPatientIdentifierList("123456");
        }

        [When(@"I encode the message")]
        public void WhenIEncodeTheMessage()
        {
            messageDsl.EncodeMessage();
        }

        [Then(@"the message can be parsed")]
        public void ThenTheMessageCanBeParsed()
        {
            messageDsl.ParseMessage();
        }
    }
}