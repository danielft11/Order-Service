using Microsoft.EntityFrameworkCore;
using Order_Service_Application.Outbox;
using Order_Service_Domain.Entities;

namespace Order_Service_Infrastructure.Persistence.DBContext
{
    public class OrderDBContext(DbContextOptions<OrderDBContext> options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public DbSet<ProcessedMessage> ProcessedMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDBContext).Assembly);
        }

    }
}
