using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ECommerceCommon.Responses;
using ECommerceCommon.Models;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ProductsDbContext _context;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, ProductsDbContext context, IProductService productService)
        {
            _logger = logger;
            _context = context;
            _productService = productService;
        }

        [HttpGet("books")]
        public async Task<ActionResult<ProductListResponse<Book>>> GetBooks(int page = 1, int pageSize = 10)
        {
            var productList = await _productService.GetListOfProductsByType<Book>(page, pageSize);
            return Ok(productList);
        }

        [HttpGet("fashion")]
        public async Task<ActionResult<ProductListResponse<Fashion>>> GetFashion(int page = 1, int pageSize = 10)
        {
            var productList = await _productService.GetListOfProductsByType<Fashion>(page, pageSize);
            return Ok(productList);
        }

        // public async Task<ActionResult<IEnumerable<IGrouping<string?, Product>>>> GetProductCategories()
        // {
        //     var products = await _context.Products.ToListAsync();

        //     if(products ==  null){
        //         return Ok(new List<IGrouping<string?, Product>>());
        //     }
        //     var categories = products.GroupBy(item => item.Category)
        //                                  .Select(group => new { category = group.Key, products = group.ToList() });
        //     return Ok(categories);
        // }

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

        private bool ProductExists(Guid id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
