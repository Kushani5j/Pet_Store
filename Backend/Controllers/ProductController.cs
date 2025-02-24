using Backend.Context;
using Backend.Dtos;
using Backend.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        //CRUD Fuctions

        //Create
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateUpdateProductDto dto)
        {
            var newProduct = new ProductEntity()
            {
                Brand = dto.Brand,
                Title = dto.Title
            };

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            return Ok("Product Saved Successfully");
        }

        //Read
        [HttpGet]
        public async Task<ActionResult<List<ProductEntity>>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<ProductEntity>>> GetProductById([FromRoute] long id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(q => q.Id == id);

            if (product is null)
            {
                return NotFound("Product not found");
            }

            return Ok(product);
        }

        //Update
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] long id, [FromBody] CreateUpdateProductDto dto)
        {
            var product = await _context.Products.FirstOrDefaultAsync(q => q.Id == id);

            if (product is null)
            {
                return NotFound("Product not found");
            }

            product.Brand = dto.Brand;
            product.Title = dto.Title;
            product.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok("Product Updated Successfully");
        }

        //Delete
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] long id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(q => q.Id == id);

            if (product is null)
            {
                return NotFound("Product not found");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Product Deleted Successfully");
        }

        [HttpGet]
        [Route("test")]
        public IActionResult TestRoute()
        {
            return Ok("Controller is working");
        }


    }
}