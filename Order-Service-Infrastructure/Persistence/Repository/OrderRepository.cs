using Microsoft.EntityFrameworkCore;
using Order_Service_Domain.Entities;
using Order_Service_Domain.Interfaces;
using Order_Service_Infrastructure.Persistence.DBContext;

namespace Order_Service_Infrastructure.Persistence.Repository
{
    public class OrderRepository(OrderDBContext context) : IOrderRepository
    {
        private readonly OrderDBContext _context = context;

        public async Task AddAsync(Order order) => await _context.Orders.AddAsync(order);

        public void UpdateOrder(Order order) => _context.Orders.Update(order);

        public async Task<Order?> GetByIdAsync(Guid id) => await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    }

}
