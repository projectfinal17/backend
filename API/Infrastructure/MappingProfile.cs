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
            CreateMap<OrderEntity, OrderDto>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(x => x.User.FirstName + " " + x.User.LastName))
                .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(x => x.User.PhoneNumber))
                .ForMember(d => d.Email, opt => opt.MapFrom(x => x.User.Email));
            CreateMap<OrderItemEntity, OrderItemDto>().
                ForMember(d => d.ProductCode , opt => opt.MapFrom(x => x.Product.Code))
                .ForMember(d => d.ProductName, opt => opt.MapFrom(x => x.Product.Name));
            CreateMap<OrderItemDto, OrderItemEntity>();

        }
    }
}
