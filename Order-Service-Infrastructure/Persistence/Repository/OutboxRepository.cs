using Order_Service_Application.Interfaces;
using Order_Service_Application.Outbox;
using Order_Service_Infrastructure.Persistence.DBContext;

namespace Order_Service_Infrastructure.Persistence.Repository
{
    public class OutboxRepository(OrderDBContext context) : IOutboxRepository
    {
        private readonly OrderDBContext _context = context;

        public async Task AddAsync(OutboxMessage message)
        {
            await _context.OutboxMessages
                .AddAsync(message);
        }
    }
}
