using TechTalk.SpecFlow;
using Turms.Model;
using Xunit;

namespace Turms.AcceptanceTest.DSL
{
    internal class MessageDsl
    {
        private readonly ScenarioContext scenarioContext;

        public MessageDsl(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        public void SetMessageString(string message)
        {
            scenarioContext["MessageString"] = message;
        }

        public void ParseMessage()
        {
            var message = scenarioContext.Get<string>("MessageString");
            var parsedMessage = Hl7Message.Parse(message);
            parsedMessage.EnsureFullyParsed();
            scenarioContext["ParsedMessage"] = parsedMessage;
        }

        public void ConfirmPatientId(string expectedValue)
        {
            var message = scenarioContext.Get<Hl7Message>("ParsedMessage");
            Assert.Equal(expectedValue, message["PID.3"].ToString());
        }

        public void ConfirmPatientName(string expectedValue)
        {
            var message = scenarioContext.Get<Hl7Message>("ParsedMessage");
            Assert.Equal(expectedValue, message["PID.5"].ToString());
        }

        public void FixMessage()
        {
            var message = scenarioContext.Get<string>("MessageString");
            message = Hl7Message.Fix(message);
            SetMessageString(message);
        }

        public void CreateNewMessage()
        {
            scenarioContext["ParsedMessage"] = new Hl7Message();
        }

        public void AddSegment(string segmentName)
        {
            scenarioContext["ParsedMessage"] = (scenarioContext["ParsedMessage"] as Hl7Message) + segmentName;
        }

        public void EncodeMessage()
        {
            SetMessageString(scenarioContext.Get<Hl7Message>("ParsedMessage").ToString());
        }

        public void SetSendingApplication(string sendingSystem)
        {
            var message = scenarioContext.Get<Hl7Message>("ParsedMessage");
            message["MSH"][2] = new Hl7Field(sendingSystem);
        }

        public void SetMessageType(string messageType)
        {
            var message = scenarioContext.Get<Hl7Message>("ParsedMessage");
            message["MSH"][8] = new Hl7Field(messageType);
        }

        public void SetSequenceNumber(string sequenceNumber)
        {
            var message = scenarioContext.Get<Hl7Message>("ParsedMessage");
            message["MSH"][12] = new Hl7Field(sequenceNumber);
        }

        public void SetPatientIdentifierList(string patientIdentifierList)
        {
            var message = scenarioContext.Get<Hl7Message>("ParsedMessage");
            message["PID"][3] = new Hl7Field(patientIdentifierList);
        }

        public void SetSurname(string surname)
        {
            //var message = scenarioContext.Get<Hl7Message>("ParsedMessage");
            //message["PID"][5][1][1] = new Hl7Component(surname);
            //scenarioContext["ParsedMessage"] = message;
        }

        public void SetGivenName(string firstname)
        {
            //var message = scenarioContext.Get<Hl7Message>("ParsedMessage");
            //message["PID"][5][1][2] = new Hl7Component(firstname);
            //scenarioContext["ParsedMessage"] = message;
        }
    }
}