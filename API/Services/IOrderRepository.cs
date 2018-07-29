using API.Entities;
using API.Infrastructure;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IOrderRepository : IGenericRepository<OrderEntity, OrderDto, OrderForCreationDto>
    {
        new Task<PagedResults<OrderDto>> GetListAsync(int offset, int limit, string keyword,
            SortOptions<OrderDto, OrderEntity> sortOptions,
            FilterOptions<OrderDto, OrderEntity> filterOptions,
            IQueryable<OrderEntity> querySearch
            );

        new Task<Guid> CreateAsync(OrderForCreationDto postDto);
        new Task<PagedResults<OrderDto>> GetAllAsync();
        Task<PagedResults<OrderDto>> GetCustomerOrder();
        Task<Guid> ChangeActiveStatusAsync(Guid id, bool isDeleted);


    }
}
