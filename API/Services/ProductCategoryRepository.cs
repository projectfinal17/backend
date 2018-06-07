using API.Entities;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Services
{
    public class ProductCategoryRepository : GenericRepository<ProductCategoryEntity, ProductCategoryDto, ProductCategoryForCreationDto>,IProductCategoryRepository
    {
        private DatabaseContext _context;
        private DbSet<ProductCategoryEntity> _entity;

        public ProductCategoryRepository(DatabaseContext context) : base(context)
        {
            _context = context;
            _entity = _context.Set<ProductCategoryEntity>();
        }
        new public async Task<Guid> CreateAsync(ProductCategoryForCreationDto creationDto)
        {
            var newProductCategory = new ProductCategoryEntity();

            var existedProductCategory = _entity.FirstOrDefault(p => p.Code == creationDto.Code);
            if (existedProductCategory != null)
            {
                throw new InvalidOperationException("ProductCategory has Code which is existed on database");
            }

            //lấy các dữ liệu vừa nhập vào entity
            foreach (PropertyInfo propertyInfo in creationDto.GetType().GetProperties())
            {
                if (newProductCategory.GetType().GetProperty(propertyInfo.Name) != null)
                {
                    newProductCategory.GetType().GetProperty(propertyInfo.Name).SetValue(newProductCategory, propertyInfo.GetValue(creationDto, null));
                }
            }
            await _entity.AddAsync(newProductCategory);

           

            var created = await _context.SaveChangesAsync();
            if (created < 1)
            {
                throw new InvalidOperationException("Database context could not create data.");
            }
            return newProductCategory.Id;
        }

        new public async Task<Guid> EditAsync(Guid id, ProductCategoryForCreationDto productCategoryDto)
        {
            var entity = await _entity.SingleOrDefaultAsync(r => r.Id == id);
            if (entity == null)
            {
                throw new InvalidOperationException("Can not find object with this Id.");
            }
            // to update DriverGroupDrivers by delete old data and create new data


            foreach (PropertyInfo propertyInfo in productCategoryDto.GetType().GetProperties())
            {
                string key = propertyInfo.Name;
                if (key != "Id" && entity.GetType().GetProperty(propertyInfo.Name) != null)
                {
                    entity.GetType().GetProperty(key).SetValue(entity, propertyInfo.GetValue(productCategoryDto, null));
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

       
    }

}
