using API.Entities;
using API.Models;
using System;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IAccessiblePageRepository : IGenericRepository<AccessiblePageEntity, AccessiblePageDto, AccessiblePageDto>
    {
        Task<String> GetRoleNamesByAccessiblePageName(string pageName);
    }
}
