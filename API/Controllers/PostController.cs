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
    [Route("Posts")]
    [Authorize]
    public class PostController : GenericController<PostEntity, PostDto, PostForCreationDto>
    {
        private readonly DatabaseContext _context;
        private readonly IPostRepository _postRepository;
        private readonly DbSet<PostEntity> _entity;

        public PostController(IPostRepository postRepository, DatabaseContext context) : base(postRepository, context)
        {
            _postRepository = postRepository;
            _context = context;
            _entity = _context.Set<PostEntity>();

        }

        public async Task<IActionResult> GetProductCategoryAsync(
             [FromQuery] int offset,
             [FromQuery] int limit,
             [FromQuery] string keyword,
             [FromQuery] SortOptions<PostDto, PostEntity> sortOptions,
             [FromQuery] FilterOptions<PostDto, PostEntity> filterOptions)
        {
            IQueryable<PostEntity> querySearch = _entity.Where(x => x.Code.Contains(keyword)
            || x.Tittle.Contains(keyword));

            var handledData = await _postRepository.GetListAsync(offset, limit, keyword, sortOptions, filterOptions, querySearch);

            var items = handledData.Items.ToArray();
            int totalSize = handledData.TotalSize;

            return Ok(new { data = items, totalSize });
        }
        
        //[HttpGet]
        //[Route("getListCodeProductCategories")]
        //public async Task<string[]> GeListAsync_Code()
        //{
        //    IQueryable<ProductCategoryEntity> query = _entity;
        //    var totalSize = await query.CountAsync();
        //    string[] Code = await _entity.Select(column => column.Code).ToArrayAsync();
        //    return Code;
        //}


    }
}
