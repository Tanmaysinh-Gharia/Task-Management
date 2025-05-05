using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.InjectionInterfaces;
using TaskManagement.Core.ViewModels.UserManagement;
using TaskManagement.Data.Entities;
namespace TaskManagement.Bussiness.MapperProfile
{
    public class UserManagementMappingProfile : Profile
    {
        public UserManagementMappingProfile()
        {
            CreateMap<User, UserModel>().ReverseMap();

            CreateMap<User, UserViewModel>();
                
            CreateMap<UserViewModel, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            CreateMap<CreateUserModel, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
