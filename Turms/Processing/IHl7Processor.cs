using Turms.Model;

namespace Turms.Processing
{
    public interface IHl7Processor
    {
        bool CanProcess(Hl7Message message);
        void ProcessMessage(Hl7Message message);
    }
}
