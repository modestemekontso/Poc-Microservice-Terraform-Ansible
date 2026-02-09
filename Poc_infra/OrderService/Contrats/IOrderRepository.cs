using OrderService.Models;

namespace OrderService.Contrats
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync(CancellationToken ct);
        Task<Order?> GetByIdAsync(int id, CancellationToken ct);
        Task<Order> AddAsync(Order customer, CancellationToken ct);
        Task UpdateAsync(Order customer, CancellationToken ct);
        Task DeleteAsync(Order customer, CancellationToken ct);
        Task<bool> ExistsAsync(int id, CancellationToken ct);
    }
}
