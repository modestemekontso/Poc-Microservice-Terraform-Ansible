using Microsoft.AspNetCore.Mvc;
using OrderService.Contrats;
using OrderService.Models;

namespace OrderService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repo;

        public OrdersController(IOrderRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetAll(CancellationToken ct)
        {
            var orders = await _repo.GetAllAsync(ct);
            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Order>> GetById(int id, CancellationToken ct)
        {
            var order = await _repo.GetByIdAsync(id, ct);
            return order is null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create([FromBody] Order order, CancellationToken ct)
        {
            var created = await _repo.AddAsync(order, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Order order, CancellationToken ct)
        {
            if (id != order.Id)
                return BadRequest("Route id and body id do not match.");

            var exists = await _repo.ExistsAsync(id, ct);
            if (!exists) return NotFound();

            await _repo.UpdateAsync(order, ct);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var order = await _repo.GetByIdAsync(id, ct);
            if (order is null) return NotFound();

            await _repo.DeleteAsync(order, ct);
            return NoContent();
        }
    }
}
