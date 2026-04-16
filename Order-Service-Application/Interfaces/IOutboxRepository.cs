using Order_Service_Application.Outbox;

namespace Order_Service_Application.Interfaces
{
    public interface IOutboxRepository
    {
        Task AddAsync(OutboxMessage outboxMessage);
    }
}
