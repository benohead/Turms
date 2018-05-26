using System.Collections.Generic;
using Turms.Model;
using Turms.Processing;

namespace Turms.AcceptanceTest.DSL
{
    internal class Hl7ProcessorMock: IHl7Processor
    {
        private readonly List<string> eventsProcessed = new List<string>();
        internal string EventType { private get; set; }

        public bool CanProcess(Hl7Message message)
        {
            return message.MessageType == EventType;
        }

        public void ProcessMessage(Hl7Message message)
        {
            eventsProcessed.Add(message.MessageType);
        }

        public bool HasProcessedEventType(string eventType)
        {
            return eventsProcessed.Contains(eventType);
        } 
    }
}