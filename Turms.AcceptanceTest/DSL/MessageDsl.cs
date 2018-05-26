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
            scenarioContext.Add("ParsedMessage", parsedMessage);
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
    }
}
