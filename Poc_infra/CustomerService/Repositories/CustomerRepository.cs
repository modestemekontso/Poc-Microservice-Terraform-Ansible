using CustomerService.Contrats;
using CustomerService.Models.Data;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace CustomerService.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _db;

        public CustomerRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Customer>> GetAllAsync(CancellationToken ct)
        {
            return await _db.Customers.AsNoTracking().ToListAsync(ct);
        }

        public async Task<Customer?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _db.Customers.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<Customer> AddAsync(Customer customer, CancellationToken ct)
        {
            await _db.Customers.AddAsync(customer, ct);
            await _db.SaveChangesAsync(ct);
            return customer;
        }

        public async Task UpdateAsync(Customer customer, CancellationToken ct)
        {
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Customer customer, CancellationToken ct)
        {
            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct)
        {
            return await _db.Customers.AnyAsync(x => x.Id == id, ct);
        }
    }
}
