using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ECommerceCommon.Models;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductsDbContext _context;

        public ProductController(ProductsDbContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? productType)
        {
            var productList = await GetProductType(productType);

            if(productList == null)
            {
                return BadRequest($"Invalid type of '{productType}'.");
            }

            return productList;
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<IGrouping<string?, Product>>>> GetProductCategories()
        {
            var products = await _context.Products.ToListAsync();

            if(products ==  null){
                return Ok(new List<IGrouping<string?, Product>>());
            }
            var categories = products.GroupBy(item => item.Category)
                                         .Select(group => new { category = group.Key, products = group.ToList() });
            return Ok(categories);
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
          if (_context.Products == null)
          {
              return Problem("Entity set 'ProductsDbContext.Products'  is null.");
          }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<ActionResult<IEnumerable<Product>>> GetProductType(string? productType)
        {
            IQueryable<Product> query = _context.Products;

            switch(productType.ToLower()){
                case "fashion":
                    return await query.OfType<Fashion>().ToListAsync();
                case "books":
                    int totalCount = await _context.Books.CountAsync();
                    int totalPages = totalCount / pageSize;
                    
                    var productsPage = await _context.Books
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

                    return await query.OfType<Book>()
                        
                        .ToListAsync();;
                default:
                    //type does not exist
                    return null;
            }

        }

        private bool ProductExists(Guid id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
