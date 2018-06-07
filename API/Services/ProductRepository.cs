using API.Entities;
using API.Infrastructure;
using API.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Services
{
    public class ProductRepository : GenericRepository<ProductEntity, ProductDto, ProductForCreationDto>,IProductRepository
    {
        private DatabaseContext _context;
        private DbSet<ProductEntity> _entity;

        public ProductRepository(DatabaseContext context) : base(context)
        {
            _context = context;
            _entity = _context.Set<ProductEntity>();
        }
        new public async Task<Guid> CreateAsync(ProductForCreationDto creationDto)
        {
            var newProduct = new ProductEntity();

            var existedProduct = _entity.FirstOrDefault(p => p.Code == creationDto.Code);
            if (existedProduct != null)
            {
                throw new InvalidOperationException("Product has Code which is existed on database");
            }

            //lấy các dữ liệu vừa nhập vào entity
            foreach (PropertyInfo propertyInfo in creationDto.GetType().GetProperties())
            {
                if (newProduct.GetType().GetProperty(propertyInfo.Name) != null)
                {
                    newProduct.GetType().GetProperty(propertyInfo.Name).SetValue(newProduct, propertyInfo.GetValue(creationDto, null));
                }
            }
            newProduct.IsActive = true;
            newProduct.CreatedDate = DateTime.Now;
            await _entity.AddAsync(newProduct);

           

            var created = await _context.SaveChangesAsync();
            if (created < 1)
            {
                throw new InvalidOperationException("Database context could not create data.");
            }
            return newProduct.Id;
        }

        new public async Task<Guid> EditAsync(Guid id, ProductForCreationDto productDto)
        {
            var entity = await _entity.SingleOrDefaultAsync(r => r.Id == id);
            if (entity == null)
            {
                throw new InvalidOperationException("Can not find object with this Id.");
            }
            // to update DriverGroupDrivers by delete old data and create new data


            foreach (PropertyInfo propertyInfo in productDto.GetType().GetProperties())
            {
                string key = propertyInfo.Name;
                if (key != "Id" && entity.GetType().GetProperty(propertyInfo.Name) != null && key != "ProductCategory")
                {
                    entity.GetType().GetProperty(key).SetValue(entity, propertyInfo.GetValue(productDto, null));
                }
            }

            _entity.Update(entity);

            var updated = await _context.SaveChangesAsync();
            if (updated < 1)
            {
                throw new InvalidOperationException("Database context could not update data.");
            }
            return id;
        }

        public new async Task<PagedResults<ProductDto>> GetListAsync(int offset, int limit, string keyword, SortOptions<ProductDto, ProductEntity> sortOptions, FilterOptions<ProductDto, ProductEntity> filterOptions, IQueryable<ProductEntity> querySearch)
        {
            IQueryable<ProductEntity> query = _entity;
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
                .ProjectTo<ProductDto>()
                .ToArrayAsync();

            return new PagedResults<ProductDto>
            {
                Items = items,
                TotalSize = size
            };
        }

        public async Task<Guid> ChangeActiveStatusAsync(Guid id, bool isActive)
        {
            var product = await _entity.SingleOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                throw new Exception("Can not find product with id=" + id);
            }
            //check isActive coincident
            if (product.IsActive == isActive)
            {
                throw new Exception("Can not change active status!");
            }
            product.IsActive = isActive;

            _entity.Update(product);
            var updated = await _context.SaveChangesAsync();
            if (updated < 1)
            {
                throw new InvalidOperationException("Database context could not updated product.");
            }

            return product.Id;
        }



    }

}
