using API.Entities;
using API.Infrastructure;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IProductCategoryRepository : IGenericRepository<ProductCategoryEntity, ProductCategoryDto, ProductCategoryForCreationDto>
    {
        new Task<PagedResults<ProductCategoryDto>> GetListAsync(int offset, int limit, string keyword,
            SortOptions<ProductCategoryDto, ProductCategoryEntity> sortOptions,
            FilterOptions<ProductCategoryDto, ProductCategoryEntity> filterOptions,
            IQueryable<ProductCategoryEntity> querySearch
            );

        new Task<Guid> CreateAsync(ProductCategoryForCreationDto productCategoryDto);

        new Task<Guid> EditAsync(Guid id, ProductCategoryForCreationDto productCategoryDto);
    }
}
