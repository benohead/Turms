using TechTalk.SpecFlow;
using Turms.Processing;
using Xunit;

namespace Turms.BDD.DSL
{
    class ProcessorDsl
    {
        private readonly ScenarioContext scenarioContext;

        public ProcessorDsl(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        public void RegisterProcessor(string eventType)
        {
            if (!scenarioContext.ContainsKey("Hl7ProcessorFarm"))
            {
                scenarioContext.Add("Hl7ProcessorFarm", new Hl7ProcessorFarm());
            }
            var hl7ProcessorFarm = scenarioContext.Get<Hl7ProcessorFarm>("Hl7ProcessorFarm");
            var processor = new Hl7ProcessorMock()
            {
                EventType = eventType
            };
            hl7ProcessorFarm.AddProcessor(processor);
            scenarioContext.Add(eventType+"Processor", processor);
        }

        public void ProcessMessage(string message)
        {
            var hl7ProcessorFarm = scenarioContext.Get<Hl7ProcessorFarm>("Hl7ProcessorFarm");
            hl7ProcessorFarm.Process(message);
        }

        public void ConfirmProcessorTriggered(string eventType)
        {
            var processor = scenarioContext.Get<Hl7ProcessorMock>(eventType + "Processor");
            Assert.True(processor.HasProcessedEventType(eventType));
        }

        internal void ConfirmProcessorNotTriggered(string eventType)
        {
            var processor = scenarioContext.Get<Hl7ProcessorMock>(eventType + "Processor");
            Assert.False(processor.HasProcessedEventType(eventType));
        }
    }
}
