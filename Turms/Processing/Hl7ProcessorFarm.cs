using System.Collections.Generic;
using Turms.Model;

namespace Turms.Processing
{
    public class Hl7ProcessorFarm
    {
        private readonly List<IHl7Processor> processors = new List<IHl7Processor>();

        public void AddProcessor(IHl7Processor processor)
        {
            processors.Add(processor);
        }

        public void Process(string message)
        {
            var hl7Message = Hl7Message.Parse(message);
            foreach (var processor in processors)
            {
                if (processor.CanProcess(hl7Message))
                {
                    processor.ProcessMessage(hl7Message);
                }
            }
        }
    }
}
