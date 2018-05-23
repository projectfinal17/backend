using API.Entities;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace API.Services
{
    public class AccessiblePageRepository : GenericRepository<AccessiblePageEntity, AccessiblePageDto, AccessiblePageDto>, IAccessiblePageRepository
    {
        private DatabaseContext _context;

        public AccessiblePageRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }
        public async Task<String> GetRoleNamesByAccessiblePageName(string pageName)
        {
            var page = await _context.AccessiblePages.SingleOrDefaultAsync(p => p.Name == pageName);
            if (page == null)
            {
                throw new Exception("Can not find page with this name");
            }
            return page.ValidRoleNames;
        }

    }
}
