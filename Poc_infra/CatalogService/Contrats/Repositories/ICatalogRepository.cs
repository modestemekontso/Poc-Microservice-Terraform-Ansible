using CatalogService.Models;

namespace CatalogService.Contrats.Repositories
{
    public interface ICatalogRepository
    {
        Task<List<Product>> GetAllAsync(CancellationToken ct);
        Task<Product?> GetByIdAsync(int id, CancellationToken ct);
        Task<Product> AddAsync(Product customer, CancellationToken ct);
        Task UpdateAsync(Product customer, CancellationToken ct);
        Task DeleteAsync(Product customer, CancellationToken ct);
        Task<bool> ExistsAsync(int id, CancellationToken ct);
    }
}
