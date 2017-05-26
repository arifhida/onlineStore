using AutoMapper;
using OnlineStore.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.API.Models.Mapping
{
    public class DomainToViewModelMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Role, RoleViewModel>();
            Mapper.CreateMap<UserInRole, UserInRoleViewModel>();
            Mapper.CreateMap<User, UserViewModel>().ForMember(x => x.Roles,
                r => r.MapFrom(o => o.UserRole));
            Mapper.CreateMap<Category, CategoryViewModel>().ForMember(x => x.Children,
                r => r.MapFrom(o => o.SubCategory));
            Mapper.CreateMap<Brand, BrandViewModel>();
            Mapper.CreateMap<Store, StoreViewModel>().ForMember(x => x.UserName,
                r => r.MapFrom(s => s.User.UserName));
            Mapper.CreateMap<Customer, CustomerViewModel>().ForMember(x => x.UserName,
                r => r.MapFrom(s => s.User.UserName));
            Mapper.CreateMap<Product, ProductViewModel>().ForMember(x => x.BrandName,
                r => r.MapFrom(s => s.Brand.BrandName))
                .ForMember(x => x.CategoryName, r => r.MapFrom(s => s.Category.CategoryName));
            Mapper.CreateMap<ProductImage, ProductImageViewModel>();
        }
    }
}
