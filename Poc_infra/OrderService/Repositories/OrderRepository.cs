using Microsoft.EntityFrameworkCore;
using OrderService.Contrats;
using OrderService.Models;
using OrderService.Models.Data;

namespace OrderService.Repositories
{
    public class OrderRepository:IOrderRepository
    {
        private readonly AppDbContext _db;

        public OrderRepository(AppDbContext db) => _db = db;

        public Task<List<Order>> GetAllAsync(CancellationToken ct)
            => _db.Orders.AsNoTracking().ToListAsync(ct);

        public Task<Order?> GetByIdAsync(int id, CancellationToken ct)
            => _db.Orders.FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<Order> AddAsync(Order customer, CancellationToken ct)
        {
            _db.Orders.Add(customer);
            await _db.SaveChangesAsync(ct);
            return customer;
        }

        public async Task UpdateAsync(Order customer, CancellationToken ct)
        {
            _db.Orders.Update(customer);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Order customer, CancellationToken ct)
        {
            _db.Orders.Remove(customer);
            await _db.SaveChangesAsync(ct);
        }

        public Task<bool> ExistsAsync(int id, CancellationToken ct)
            => _db.Orders.AnyAsync(x => x.Id == id, ct);
    }
}

