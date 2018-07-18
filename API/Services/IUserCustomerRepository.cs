using API.Entities;
using API.Infrastructure;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IUserCustomerRepository
    {
        Task<Guid> CreateAsync(UserForCreationDto creationDto);
        Task<PagedResults<UserDto>> GetListAsync(int offset, int limit, string keyword,
        SortOptions<UserDto, UserEntity> sortOptions, FilterOptions<UserDto, UserEntity> filterOptions,
        IQueryable<UserEntity> querySearch
        );

        Task<UserDto> GetCurrentUser();
    }
}
