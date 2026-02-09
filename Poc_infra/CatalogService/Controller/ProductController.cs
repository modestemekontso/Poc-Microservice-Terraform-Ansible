using CatalogService.Contrats.Repositories;
using CatalogService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controller
{
    public class ProductController : ControllerBase
    {
        private readonly ICatalogRepository _repo;

        public ProductController(ICatalogRepository repo)
        {
            _repo = repo;
        }

        // GET api/products
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAll(CancellationToken ct)
        {
            var products = await _repo.GetAllAsync(ct);
            return Ok(products);
        }

        // GET api/products/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetById(int id, CancellationToken ct)
        {
            var product = await _repo.GetByIdAsync(id, ct);
            return product is null ? NotFound() : Ok(product);
        }

        // POST api/products
        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product, CancellationToken ct)
        {
            var created = await _repo.AddAsync(product, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT api/products/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Product product, CancellationToken ct)
        {
            if (id != product.Id)
                return BadRequest("Id mismatch");

            await _repo.UpdateAsync(product, ct);
            return NoContent();
        }

        // DELETE api/products/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var product = await _repo.GetByIdAsync(id, ct);
            if (product is null)
                return NotFound();

            await _repo.DeleteAsync(product, ct);
            return NoContent();
        }
    }
}
