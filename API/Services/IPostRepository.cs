using API.Entities;
using API.Infrastructure;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IPostRepository : IGenericRepository<PostEntity, PostDto, PostForCreationDto>
    {
        new Task<PagedResults<PostDto>> GetListAsync(int offset, int limit, string keyword,
            SortOptions<PostDto, PostEntity> sortOptions,
            FilterOptions<PostDto, PostEntity> filterOptions,
            IQueryable<PostEntity> querySearch
            );

        new Task<Guid> CreateAsync(PostForCreationDto postDto);

        new Task<Guid> EditAsync(Guid id, PostForCreationDto postDto);
    }
}
