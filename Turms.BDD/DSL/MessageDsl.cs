using TechTalk.SpecFlow;
using Turms.Model;
using Xunit;

namespace Turms.BDD.DSL
{
    internal static class MessageDsl
    {
        public static void SetMessageString(string message)
        {
            ScenarioContext.Current.Add("MessageString", message);
        }

        public static void ParseMessage()
        {
            var message = ScenarioContext.Current.Get<string>("MessageString");
            var parsedMessage = Hl7Message.Parse(message);
            ScenarioContext.Current.Add("ParsedMessage", parsedMessage);
        }

        public static void ConfirmPatientId(string expectedValue)
        {
            var message = ScenarioContext.Current.Get<Hl7Message>("ParsedMessage");
            Assert.Equal(expectedValue, message["PID.3"].ToString());
        }

        public static void ConfirmPatientName(string expectedValue)
        {
            var message = ScenarioContext.Current.Get<Hl7Message>("ParsedMessage");
            Assert.Equal(expectedValue, message["PID.5"].ToString());
        }
    }
}
