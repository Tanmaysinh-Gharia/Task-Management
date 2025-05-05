using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.InjectionInterfaces;
using TaskManagement.Core.ViewModels.Login;
using TaskManagement.Core.ViewModels.UserManagement;
using TaskManagement.Data.Entities;
namespace TaskManagement.Bussiness.MapperProfile
{
    public class LoginMappingProfile : Profile
    {
        public LoginMappingProfile()
        {
            CreateMap<RefreshToken, RefreshTokenModel>().ReverseMap();
        }
    }
}
