using Mapster;
using MapsterMapper;
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
    public class UserManagementMappingProfile : IMapperProfile
    {
        public void MapProfile()
        {
            TypeAdapterConfig<User, UserModel>.NewConfig().TwoWays();


            // Forward: Entity to ViewModel (Id & Email included)
            TypeAdapterConfig<User, UserViewModel>.NewConfig();

            // Reverse: ViewModel to Entity (Id & Email ignored)
            TypeAdapterConfig<UserViewModel, User>.NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.Email)
                .Ignore(dest => dest.PasswordHash)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.Role);

            TypeAdapterConfig<CreateUserModel, User>.NewConfig()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CreatedAt)
                .Ignore(dest => dest.PasswordHash)
                .Ignore(dest => dest.Role);
        }
    }
}
