using API.Entities;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("Products")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly DatabaseContext _context;

        public ProductController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CreateProductAsync([FromBody] ProductForCreationDto productForCreationDto)
        {
            ProductEntity productEntity = new ProductEntity
            {
                Name = productForCreationDto.Name,
                ProductCategoryId = productForCreationDto.ProductCategoryId
            };
            try
            {
                await _context.Products.AddAsync(productEntity);

                await _context.SaveChangesAsync();

                return Created("", productEntity.Id);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
         
        }
    }
}
