using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.Entities;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class UserCutomerRepository : IUserCustomerRepository
    {
        private DatabaseContext _context;
        private DbSet<UserEntity> _entity;
        private IMapper _mapper;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserCutomerRepository(DatabaseContext context, UserManager<UserEntity> userManager,
                IHttpContextAccessor httpContextAccessor
         )
        {
            _userManager = userManager;
            _context = context;
            _entity = _context.Set<UserEntity>();
            _httpContextAccessor = httpContextAccessor;

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserEntity, UserDto>();
            });

            _mapper = config.CreateMapper();
        }
        public async Task<Guid> CreateAsync(UserForCreationDto creationDto)
        {
            UserEntity newUser = Activator.CreateInstance<UserEntity>();
            newUser.IsActive = true;

            foreach (PropertyInfo propertyInfo in creationDto.GetType().GetProperties())
            {
                if (newUser.GetType().GetProperty(propertyInfo.Name) != null)
                {
                    newUser.GetType().GetProperty(propertyInfo.Name).SetValue(newUser, propertyInfo.GetValue(creationDto, null));
                }

            }

            newUser.UserName = creationDto.Email;
            //RoleName tam thoi khong gan = User duoc, nen tren giao dien tao TK KH cua de ngam la 1 mang "Rolenames" : ["USER"]
            await _userManager.CreateAsync(newUser, creationDto.Password);
            foreach (string roleName in creationDto.RoleNames)
            {
                if (roleName == "USER")
                {
                    await _userManager.AddToRoleAsync(newUser, roleName);
                }
            }

            await _userManager.UpdateAsync(newUser);

            return newUser.Id;
        }
    }
}
