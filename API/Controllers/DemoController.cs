using API.Entities;
using API.Infrastructure;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("Demos")]
    [Authorize]
    public class DemoController : GenericController<DemoEntity, DemoDto, DemoDto>
    {
        private readonly DatabaseContext _context;
        private readonly IGenericRepository<DemoEntity, DemoDto, DemoDto> _genericRepository;
        private readonly DbSet<DemoEntity> _entity;
        private static readonly HttpClient client = new HttpClient();

      
        public DemoController(IGenericRepository<DemoEntity, DemoDto, DemoDto> genericRepository, DatabaseContext context) : base(genericRepository, context)
        {
            _genericRepository = genericRepository;
            _context = context;
            _entity = _context.Set<DemoEntity>();

        }
        public async Task<IActionResult> GeListAsync(
         [FromQuery] int offset,
         [FromQuery] int limit,
         [FromQuery] SortOptions<DemoDto, DemoEntity> sortOptions,
         [FromQuery] FilterOptions<DemoDto, DemoEntity> DemoOptions,
         [FromQuery] string keyword,
         [FromQuery] bool isSoldOut = false
         )
        {
            IQueryable<DemoEntity> querySearch = _entity;
            if (keyword != null)
            {
                querySearch = _entity.Where(
                x => x.Name.Contains(keyword)
                || x.Type.Contains(keyword)
                );
            }

            var handledData = await _genericRepository.GetListAsync(offset, limit, keyword, sortOptions, DemoOptions, querySearch);
            var items = handledData.Items.ToArray();
            int totalSize = handledData.TotalSize;
            return Ok(new { data = items, totalSize });
        }
      

    }
}
