using API.Entities;
using API.Infrastructure;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("AccessiblePages")]
    [Authorize]
    public class AccessiblePagesController : GenericController<AccessiblePageEntity, AccessiblePageDto, AccessiblePageDto>
    {
        private readonly IAccessiblePageRepository _pageRepository;
        private readonly DatabaseContext _context;


        public AccessiblePagesController(IAccessiblePageRepository pageGenericRepository, DatabaseContext context)
            : base(pageGenericRepository, context)
        {
            _pageRepository = pageGenericRepository;
            _context = context;
        }

        public async Task<IActionResult> GetAccessiblePagesAsync(
             [FromQuery] int offset,
             [FromQuery] int limit,
             [FromQuery] string keyword,
             [FromQuery] SortOptions<AccessiblePageDto, AccessiblePageEntity> sortOptions,
             [FromQuery] FilterOptions<AccessiblePageDto, AccessiblePageEntity> filterOptions)
        {
            IQueryable<AccessiblePageEntity> querySearch = _context.AccessiblePages.Where(x => x.Name.Contains(keyword));

            var handledData = await _pageRepository.GetListAsync(offset, limit, keyword, sortOptions, filterOptions, querySearch);

            var items = handledData.Items.ToArray();
            int totalSize = handledData.TotalSize;

            return Ok(new { data = items, totalSize });
        }

        [HttpGet]
        [Route("roleNames/{pageName}")]
        public async Task<IActionResult> GetRoleNamesByAccessiblePageName(string pageName)
        {
            var roleNames = await _pageRepository.GetRoleNamesByAccessiblePageName(pageName);
            return Ok(new { data = roleNames });
        }

    }
}
