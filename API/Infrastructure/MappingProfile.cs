using API.Entities;
using API.Models;
using AutoMapper;

namespace API.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DemoEntity, DemoDto>();
            CreateMap<AccessiblePageEntity, AccessiblePageDto>();
            CreateMap<RoleEntity, RoleDto>();
            CreateMap<UserEntity, UserDto>();
            CreateMap<ProductCategoryEntity, ProductCategoryDto>();
            CreateMap<ProductEntity, ProductDto>();
            CreateMap<PostEntity, PostDto>();

        }
    }
}
