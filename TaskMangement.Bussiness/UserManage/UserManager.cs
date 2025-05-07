
using AutoMapper;
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

        /// <summary>
        /// Retrieves all users and maps them to view models.
        /// </summary>
        public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
        {
            return _mapper.Map<IEnumerable<UserViewModel>>( await _userRepository.GetAllUsersAsync() );
        }

        /// <summary>
        /// Gets a user by their ID and maps to UserModel. Returns null if not found.
        /// </summary>
        public async Task<UserModel?> GetUserByIdAsync(int id)
        {
            User? user = await _userRepository.GetUserByIdAsync(id);
            return user == null ? null : _mapper.Map<UserModel>(user);
        }

        /// <summary>
        /// Gets a user by their email and maps to UserModel. Returns null if not found.
        /// </summary>
        public async Task<UserModel?> GetUserByEmailAsync(string email)
        {
            User? user = await _userRepository.GetUserByEmailAsync(email);
            return user == null ? null : _mapper.Map<UserModel>(user);
        }


        /// <summary>
        /// Adds a new user by mapping from CreateUserModel and hashing the password. 
        /// Sets default role and creation time.
        /// </summary>
        public async Task AddUserAsync(CreateUserModel model)
        {
            User entity = _mapper.Map<User>(model);
            entity.PasswordHash = _hashing.HashPassword(model.Password);
            entity.CreatedAt = DateTime.Now;
            entity.Role = Role.User; // default role if needed

            await _userRepository.AddAsync(entity);
        }

        
        /// <summary>
        /// Returns user details as a UserViewModel for display purposes. Throws if not found.
        /// </summary>
        public async Task<UserViewModel> ShowUserByIdAsync(int id)
        {
            User? user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) throw new Exception("User not found");
            return _mapper.Map<UserViewModel>(user);
        }
        
        /// <summary>
        /// Updates user data using UserViewModel. Only non-sensitive fields are updated.
        /// </summary>
        public async Task UpdateUserAsync(UserViewModel userModel)
        {
            User? user = await _userRepository.GetUserByIdAsync(userModel.Id);
            if (user == null) throw new Exception("User not found");

            // Only update allowed fields
            user.UserName = userModel.UserName;
            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.PhoneNumber = userModel.PhoneNumber;
            user.IsActive = userModel.IsActive;

            await _userRepository.UpdateAsync(user);
        }


        /// <summary>
        /// Deletes a user by ID if the user exists in the database.
        /// </summary>
        public async Task DeleteUserAsync(int id)
        {
            User? user = await _userRepository.GetUserByIdAsync(id);
            if (user != null)
            {
                await _userRepository.DeleteAsync(user);
            }
        }

        /// <summary>
        /// Checks if a user exists by ID.
        /// </summary>
        public async Task<bool> UserExistsAsync(int id)
        {
            return await _userRepository.ExistsAsync(id);
        }

    }
}
