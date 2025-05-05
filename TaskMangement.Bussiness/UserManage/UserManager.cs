using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Enums;
using TaskManagement.Core.Helpers;
using TaskManagement.Core.ViewModels.UserManagement;
using TaskManagement.Data.Entities;
using TaskManagement.Data.Repositories.UserRepo;

namespace TaskManagement.Bussiness.UserManage
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly Hashing _hashing;

        public UserManager(IUserRepository userRepository,IMapper mapper, Hashing hashing)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _hashing = hashing;
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
        {
            return _mapper.Map<IEnumerable<UserViewModel>>( await _userRepository.GetAllUsersAsync() );
        }

        public async Task<UserModel?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user == null ? null : _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            return user == null ? null : _mapper.Map<UserModel>(user);
        }

        public async Task AddUserAsync(UserModel userModel)
        {
            var user = _mapper.Map<User>(userModel);
            await _userRepository.AddAsync(user);
        }

        public async Task AddUserAsync(CreateUserModel model)
        {
            var entity = _mapper.Map<User>(model);
            entity.PasswordHash = _hashing.HashPassword(model.Password);
            entity.CreatedAt = DateTime.Now;
            entity.Role = Role.User; // default role if needed

            await _userRepository.AddAsync(entity);
        }


        public async Task UpdateUserAsync(UserModel userModel)
        {
            var user = _mapper.Map<User>(userModel);
            await _userRepository.UpdateAsync(user);
        }

        public async Task<UserViewModel> ShowUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) throw new Exception("User not found");
            return _mapper.Map<UserViewModel>(user);
        }

        public async Task UpdateUserAsync(UserViewModel userModel)
        {
            var user = await _userRepository.GetUserByIdAsync(userModel.Id);
            if (user == null) throw new Exception("User not found");

            // Only update allowed fields
            user.UserName = userModel.UserName;
            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.PhoneNumber = userModel.PhoneNumber;
            user.IsActive = userModel.IsActive;

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user != null)
            {
                await _userRepository.DeleteAsync(user);
            }
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            return await _userRepository.ExistsAsync(id);
        }

    }
}
