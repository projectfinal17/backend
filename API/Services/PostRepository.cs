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
    public class PostRepository : GenericRepository<PostEntity, PostDto, PostForCreationDto>,IPostRepository
    {
        private DatabaseContext _context;
        private DbSet<PostEntity> _entity;

        public PostRepository(DatabaseContext context) : base(context)
        {
            _context = context;
            _entity = _context.Set<PostEntity>();
        }
        new public async Task<Guid> CreateAsync(PostForCreationDto creationDto)
        {
            var newPost = new PostEntity();

            var existedPost = _entity.FirstOrDefault(p => p.Code == creationDto.Code);
            if (existedPost != null)
            {
                throw new InvalidOperationException("Post has Code which is existed on database");
            }

            //lấy các dữ liệu vừa nhập vào entity
            foreach (PropertyInfo propertyInfo in creationDto.GetType().GetProperties())
            {
                if (newPost.GetType().GetProperty(propertyInfo.Name) != null)
                {
                    newPost.GetType().GetProperty(propertyInfo.Name).SetValue(newPost, propertyInfo.GetValue(creationDto, null));
                }
            }
            await _entity.AddAsync(newPost);

           

            var created = await _context.SaveChangesAsync();
            if (created < 1)
            {
                throw new InvalidOperationException("Database context could not create data.");
            }
            return newPost.Id;
        }

        new public async Task<Guid> EditAsync(Guid id, PostForCreationDto postDto)
        {
            var entity = await _entity.SingleOrDefaultAsync(r => r.Id == id);
            if (entity == null)
            {
                throw new InvalidOperationException("Can not find object with this Id.");
            }
            // to update DriverGroupDrivers by delete old data and create new data


            foreach (PropertyInfo propertyInfo in postDto.GetType().GetProperties())
            {
                string key = propertyInfo.Name;
                if (key != "Id" && entity.GetType().GetProperty(propertyInfo.Name) != null)
                {
                    entity.GetType().GetProperty(key).SetValue(entity, propertyInfo.GetValue(postDto, null));
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
