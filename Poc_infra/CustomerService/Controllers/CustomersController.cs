using CustomerService.Contrats;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;

namespace CustomerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _repository;

        public CustomersController(ICustomerRepository repository)
        {
            _repository = repository;
        }

        // GET api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAll(CancellationToken ct)
        {
            var customers = await _repository.GetAllAsync(ct);
            return Ok(customers);
        }

        // GET api/customers/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Customer>> GetById(int id, CancellationToken ct)
        {
            var customer = await _repository.GetByIdAsync(id, ct);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        // POST api/customers
        [HttpPost]
        public async Task<ActionResult<Customer>> Create(Customer customer, CancellationToken ct)
        {
            var created = await _repository.AddAsync(customer, ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created
            );
        }

        // PUT api/customers/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Customer customer, CancellationToken ct)
        {
            if (id != customer.Id)
                return BadRequest("Id mismatch");

            var exists = await _repository.ExistsAsync(id, ct);
            if (!exists)
                return NotFound();

            await _repository.UpdateAsync(customer, ct);
            return NoContent();
        }

        // DELETE api/customers/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var customer = await _repository.GetByIdAsync(id, ct);

            if (customer == null)
                return NotFound();

            await _repository.DeleteAsync(customer, ct);
            return NoContent();
        }
    }
}
