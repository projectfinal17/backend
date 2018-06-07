using API.Entities;
using API.Infrastructure;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("ProductCategories")]
    [Authorize]
    public class ProductCategoryController : GenericController<ProductCategoryEntity, ProductCategoryDto, ProductCategoryForCreationDto>
    {
        private readonly DatabaseContext _context;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly DbSet<ProductCategoryEntity> _entity;

        public ProductCategoryController(IProductCategoryRepository productCategoryRepository, DatabaseContext context) : base(productCategoryRepository, context)
        {
            _productCategoryRepository = productCategoryRepository;
            _context = context;
            _entity = _context.Set<ProductCategoryEntity>();

        }

        public async Task<IActionResult> GetProductCategoryAsync(
             [FromQuery] int offset,
             [FromQuery] int limit,
             [FromQuery] string keyword,
             [FromQuery] SortOptions<ProductCategoryDto, ProductCategoryEntity> sortOptions,
             [FromQuery] FilterOptions<ProductCategoryDto, ProductCategoryEntity> filterOptions)
        {
            IQueryable<ProductCategoryEntity> querySearch = _entity.Where(x => x.Code.Contains(keyword)
            || x.Name.Contains(keyword));

            var handledData = await _productCategoryRepository.GetListAsync(offset, limit, keyword, sortOptions, filterOptions, querySearch);

            var items = handledData.Items.ToArray();
            int totalSize = handledData.TotalSize;

            return Ok(new { data = items, totalSize });
        }
        
        [HttpGet]
        [Route("getListCodeProductCategories")]
        public async Task<string[]> GeListAsync_Code()
        {
            IQueryable<ProductCategoryEntity> query = _entity;
            var totalSize = await query.CountAsync();
            string[] Code = await _entity.Select(column => column.Code).ToArrayAsync();
            return Code;
        }


    }
}
