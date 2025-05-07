using AutoMapper;
using TaskManagement.Core.ViewModels.Login;
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
