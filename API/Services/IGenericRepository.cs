using API.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IGenericRepository<TEntity, TDto , TCreationDto>
    {
        Task<TDto> GetSingleAsync(Guid id);
        Task<PagedResults<TDto>> GetListAsync(int offset, int limit, string keyword, 
            SortOptions<TDto, TEntity> sortOptions, FilterOptions<TDto, TEntity> filterOptions,
            IQueryable<TEntity> querySearch
            );
        Task<PagedResults<TDto>> GetAllAsync();
        Task<Guid> CreateAsync(TCreationDto creationDto);
        Task<Guid> EditAsync(Guid id, TCreationDto creationDto);
        Task<Guid> DeleteAsync(Guid id);

    }
}
