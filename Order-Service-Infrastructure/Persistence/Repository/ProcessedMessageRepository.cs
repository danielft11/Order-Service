using Microsoft.EntityFrameworkCore;
using Order_Service_Application.Interfaces;
using Order_Service_Application.Outbox;
using Order_Service_Infrastructure.Persistence.DBContext;

namespace Order_Service_Infrastructure.Persistence.Repository
{
    public class ProcessedMessageRepository(OrderDBContext context) : IProcessedMessageRepository
    {
        private readonly OrderDBContext _context = context;

        public async Task<bool> Exists(Guid messageId)
        {
            return await _context.ProcessedMessages
                .AnyAsync(x => x.Id == messageId);
        }

        public async Task AddAsync(ProcessedMessage message)
        {
            await _context.ProcessedMessages
               .AddAsync(message);
        }

    }
}
