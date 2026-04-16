using Order_Service_Application.Outbox;

namespace Order_Service_Application.Interfaces
{
    public interface IProcessedMessageRepository
    {
        Task<bool> Exists(Guid messageId);
        Task AddAsync(ProcessedMessage message);
    }
}
