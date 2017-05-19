using AutoMapper;
using OnlineStore.Model;
using OnlineStore.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.API.Models.Mapping
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<UserViewModel, User>().ForMember(x => x.Gender,
                m => m.MapFrom(r => Enum.Parse(typeof(Gender), r.Gender.ToString())))
                .ForMember(x => x.UserRole,
                m => m.MapFrom(r => r.Roles));
            Mapper.CreateMap<RoleViewModel, Role>();
            Mapper.CreateMap<UserInRoleViewModel, UserInRole>();
            Mapper.CreateMap<CategoryViewModel, Category>();
            Mapper.CreateMap<BrandViewModel, Brand>();
            Mapper.CreateMap<StoreViewModel, Store>();
            Mapper.CreateMap<CustomerViewModel, Customer>();
        }
    }
}
