using API.Entities;
using API.Helpers;
using API.Infrastructure;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


namespace API.Services
{
    public class OrderRepository : GenericRepository<OrderEntity, OrderDto, OrderForCreationDto>, IOrderRepository
    {
        private DatabaseContext _context;
        private DbSet<OrderEntity> _order;
        private IMapper _mapper;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;



        public OrderRepository(DatabaseContext context, UserManager<UserEntity> userManager, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _order = _context.Set<OrderEntity>();
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        new public async Task<Guid> CreateAsync(OrderForCreationDto creationDto)
        {
            var newOrder = new OrderEntity();
            // map 
            var orderItems = Mapper.Map<List<OrderItemEntity>>(creationDto.OrderItems);
            newOrder.OrderItems = orderItems;
            newOrder.TotalMoney = orderItems.Sum(s => s.TotalMoney);
            // lay usser hien tai
            newOrder.UserId = (await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User)).Id;
            newOrder.CreatedDate = DateTime.Now;
            newOrder.Address = creationDto.Address;
            newOrder.isDeleted = false;
            //tao ma code
            EnhanceCodeGeneratorHelper<OrderEntity> codeGeneratorHelper = new EnhanceCodeGeneratorHelper<OrderEntity>(_context);
            newOrder.Code = await codeGeneratorHelper.ReturnCode(CONSTANT.BILL_PREFIX, CONSTANT.GENERATED_NUMBER_LENGTH);

            await _order.AddAsync(newOrder);

            var orderShow = newOrder;


            var created = await _context.SaveChangesAsync();
            if (created < 1)
            {
                throw new InvalidOperationException("Database context could not create data.");
            }
            return newOrder.Id;
        }


        public new async Task<PagedResults<OrderDto>> GetListAsync(int offset, int limit, string keyword,
            SortOptions<OrderDto, OrderEntity> sortOptions, FilterOptions<OrderDto, OrderEntity> filterOptions,
            IQueryable<OrderEntity> querySearch)
        {
            IQueryable<OrderEntity> query = _order;
            query = sortOptions.Apply(query);
            query = filterOptions.Apply(query);
            if (keyword != null)
            {
                query = querySearch;
            }

            var size = await query.CountAsync();

            var items = await query
                .Skip(offset * limit)
                .Take(limit)
                .ProjectTo<OrderDto>()
                .ToArrayAsync();

            return new PagedResults<OrderDto>
            {
                Items = items,
                TotalSize = size
            };
        }

        public async Task<PagedResults<OrderDto>> GetCustomerOrder()
        {

            var userId = (await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User)).Id;
            IQueryable<OrderEntity> oders = _context.Orders.AsNoTracking().Where(w => w.UserId == userId);
            
            if (oders == null)
            {
                throw new InvalidOperationException("Can not find object with this Id.");
            }
            var items = await oders
                .Skip(0)
                .Take(10)
                .ProjectTo<OrderDto>()
                .ToListAsync();
            return new PagedResults<OrderDto>
            {
                Items = items,
                TotalSize = items.Count
            };
        }

        public async Task<Guid> ChangeActiveStatusAsync(Guid id, bool isDeleted)
        {
            var order = await _order.SingleOrDefaultAsync(p => p.Id == id);
            if (order == null)
            {
                throw new Exception("Can not find product with id=" + id);
            }
            //check isActive coincident
            if (order.isDeleted == isDeleted)
            {
                throw new Exception("Can not change active status!");
            }
            order.isDeleted = isDeleted;

            _order.Update(order);
            var updated = await _context.SaveChangesAsync();
            if (updated < 1)
            {
                throw new InvalidOperationException("Database context could not updated product.");
            }

            return order.Id;
        }

        //new public async Task<Guid> EditAsync(Guid id, ProductForCreationDto productDto)
        //{
        //    var entity = await _entity.SingleOrDefaultAsync(r => r.Id == id);
        //    if (entity == null)
        //    {
        //        throw new InvalidOperationException("Can not find object with this Id.");
        //    }
        //    // to update DriverGroupDrivers by delete old data and create new data


        //    foreach (PropertyInfo propertyInfo in productDto.GetType().GetProperties())
        //    {
        //        string key = propertyInfo.Name;
        //        if (key != "Id" && entity.GetType().GetProperty(propertyInfo.Name) != null && key != "ProductCategory")
        //        {
        //            entity.GetType().GetProperty(key).SetValue(entity, propertyInfo.GetValue(productDto, null));
        //        }
        //    }

        //    _entity.Update(entity);

        //    var updated = await _context.SaveChangesAsync();
        //    if (updated < 1)
        //    {
        //        throw new InvalidOperationException("Database context could not update data.");
        //    }
        //    return id;
        //}


    }

}
