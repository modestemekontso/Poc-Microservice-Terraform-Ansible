using CatalogService.Contrats.Repositories;
using CatalogService.Models;
using CatalogService.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Repositories
{
    public class CatalogRepository:ICatalogRepository
    {
        private readonly AppDbContext _db;

        public CatalogRepository(AppDbContext db) => _db = db;

        public Task<List<Product>> GetAllAsync(CancellationToken ct)
            => _db.Products.AsNoTracking().ToListAsync(ct);

        public Task<Product?> GetByIdAsync(int id, CancellationToken ct)
            => _db.Products.FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<Product> AddAsync(Product product, CancellationToken ct)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync(ct);
            return product;
        }

        public async Task UpdateAsync(Product product, CancellationToken ct)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Product product, CancellationToken ct)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync(ct);
        }

        public Task<bool> ExistsAsync(int id, CancellationToken ct)
            => _db.Products.AnyAsync(x => x.Id == id, ct);
    }
}

