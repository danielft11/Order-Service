using Order_Service_Domain.Entities;

namespace Order_Service_Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id);
        Task AddAsync(Order order);
        void UpdateOrder(Order order);
        Task SaveChangesAsync();
    }
}
