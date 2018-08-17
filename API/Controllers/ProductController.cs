using API.Entities;
using API.Infrastructure;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("Products")]
    [Authorize]
    public class ProductController : GenericController<ProductEntity,ProductDto,ProductForCreationDto>
    {
        private readonly DatabaseContext _context;
        private readonly IProductRepository _productRepository;
        private readonly DbSet<ProductEntity> _entity;

        public ProductController(IProductRepository productRepository, DatabaseContext context) : base(productRepository, context)
        {
            _productRepository = productRepository;
            _context = context;
            _entity = _context.Set<ProductEntity>();

        }

        public async Task<IActionResult> GetProductsAsync(
             [FromQuery] int offset,
             [FromQuery] int limit,
             [FromQuery] string keyword,
             [FromQuery] SortOptions<ProductDto, ProductEntity> sortOptions,
             [FromQuery] FilterOptions<ProductDto, ProductEntity> filterOptions)
        {
            IQueryable<ProductEntity> querySearch = _entity.Where(x => x.Code.Contains(keyword)
            || x.Name.Contains(keyword));

            var handledData = await _productRepository.GetListAsync(offset, limit, keyword, sortOptions, filterOptions, querySearch);

            var items = handledData.Items.ToArray();
            int totalSize = handledData.TotalSize;

            return Ok(new { data = items, totalSize });
        }
        [HttpGet]
        [Route("CheckCodeExist")]
        public async Task<Boolean> CheckCodeExistAsync([FromQuery] string code)
        {
            var entity = await _entity.SingleOrDefaultAsync(r => r.Code == code);
            if (entity == null)
            {
                return false;
            }
            else
                return true;
        }

        [HttpPut("isActive/{id}/{isActive}")]
        public async Task<IActionResult> ChangeActiveStatus(Guid id, bool isActive)
        {
            try
            {
                var returnId = await _productRepository.ChangeActiveStatusAsync(id, isActive);
                return Ok(new { id = returnId });
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse(ex.Message));
            }
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("ListProduct")]
        public async Task<IActionResult> GetListForCustomerAsync(
         [FromQuery] int offset,
         [FromQuery] int limit,
         [FromQuery] SortOptions<ProductDto, ProductEntity> sortOptions,
         [FromQuery] FilterOptions<ProductDto, ProductEntity> DemoOptions,
         [FromQuery] string keyword,
         [FromQuery] bool isSoldOut = false
         )
        {
            IQueryable<ProductEntity> querySearch = _entity;
            if (keyword != null)
            {
                querySearch = _entity.Where(
                x => x.Name.Contains(keyword) || x.Code.Contains(keyword)
                );
            }

            var handledData = await _productRepository.GetListAsync(offset, limit, keyword, sortOptions, DemoOptions, querySearch);
            var items = handledData.Items.ToArray();
            int totalSize = handledData.TotalSize;
            return Ok(new { data = items, totalSize });
        }

        [AllowAnonymous]
        [Route("ListProductAll")]
        public async Task<IActionResult> GetAllForCustomerAsync()
        {
            var handledData = await _productRepository.GetAllAsync();

            var items = handledData.Items.ToArray();
            int totalSize = handledData.TotalSize;

            return Ok(new { data = items, totalSize });
        }

        [Route("GetDetailAsync/{id}")]
        public async Task<IActionResult> GetDetailAsync(Guid id)
        {
            var handledData = await _productRepository.GetSingleAsync(id);

            return Ok(new { data = handledData });
        }
    }
}
