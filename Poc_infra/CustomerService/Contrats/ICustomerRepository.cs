using OrderService.Models;

namespace CustomerService.Contrats
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync(CancellationToken ct);
        Task<Customer?> GetByIdAsync(int id, CancellationToken ct);
        Task<Customer> AddAsync(Customer customer, CancellationToken ct);
        Task UpdateAsync(Customer customer, CancellationToken ct);
        Task DeleteAsync(Customer customer, CancellationToken ct);
        Task<bool> ExistsAsync(int id, CancellationToken ct);
    }
}
