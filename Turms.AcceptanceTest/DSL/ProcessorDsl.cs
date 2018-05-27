using TechTalk.SpecFlow;
using Turms.Processing;
using Xunit;

namespace Turms.AcceptanceTest.DSL
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
            if (!scenarioContext.ContainsKey("Hl7ProcessorQueue"))
            {
                scenarioContext.Add("Hl7ProcessorQueue", new Hl7ProcessorQueue());
            }
            var hl7ProcessorQueue = scenarioContext.Get<Hl7ProcessorQueue>("Hl7ProcessorQueue");
            var processor = new Hl7ProcessorMock()
            {
                EventType = eventType
            };
            hl7ProcessorQueue.AddProcessor(processor);
            scenarioContext.Add(eventType + "Processor", processor);
        }

        public void ProcessMessage(string message)
        {
            var hl7ProcessorQueue = scenarioContext.Get<Hl7ProcessorQueue>("Hl7ProcessorQueue");
            hl7ProcessorQueue.Process(message);
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