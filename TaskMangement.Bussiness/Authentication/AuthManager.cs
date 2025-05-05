using MapsterMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.Helpers;
using TaskManagement.Core.ViewModels.Login;
using TaskManagement.Core.ViewModels.UserManagement;
using TaskManagement.Data.Entities;
using TaskManagement.Data.Repositories.RefreshTokenRepo;
using TaskManagement.Data.Repositories.UserRepo;

namespace TaskManagement.Bussiness.Authentication
{
    public class AuthManager : IAuthManager
    {
        private readonly JWTGenerator _jwt;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepo;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly Hashing _hashing;
        private readonly IMapper _mapper;

        public AuthManager(Hashing hashing, JWTGenerator jwt, IHttpContextAccessor httpContextAccessor, IUserRepository userRepo, IRefreshTokenRepository refreshTokenRepo, IMapper mapper)
        {
            _hashing = hashing;
            _jwt = jwt;
            _httpContextAccessor = httpContextAccessor;
            _userRepo = userRepo;
            _refreshTokenRepo = refreshTokenRepo;
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> LoginAsync(string email, string password)
        {
            var userResponse = await _userRepo.GetUserByEmailAsync(email);
            UserModel user = userResponse == null ? null : _mapper.Map<UserModel>(userResponse);

            if (user == null || !_hashing.VerifyPassword(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException();
            }

            string ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            var accessToken = _jwt.GenerateAccessToken(email, user.Id.ToString(), ip,user.Role);

            RefreshTokenModel refreshToken = new RefreshTokenModel
            {
                Token = _jwt.GenerateRefreshToken(),
                Expires = DateTime.Now.AddDays(7),
                CreatedFromIp = ip,
                IsRevoked = false,
                UserId = user.Id
            };

            await _refreshTokenRepo.SaveTokenAsync(user.Id, _mapper.Map<RefreshToken>( refreshToken));
            return new AuthenticationResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
            };
        }

        public async Task<AuthenticationResponse> RefreshTokenAsync(string token)
        {
            var refreshToken = _mapper.Map<RefreshTokenModel>( await _refreshTokenRepo.GetValidTokenAsync(token));
            if (refreshToken == null)
            {
                throw new UnauthorizedAccessException();
            }
            var user =_mapper.Map<UserModel>( await _userRepo.GetUserByIdAsync(refreshToken.UserId));
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }
            string ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

            var accessToken = _jwt.GenerateAccessToken(user.Email, user.Id.ToString(), ip, user.Role);
            refreshToken.IsRevoked = true;

            await _refreshTokenRepo.UpdateAsync(_mapper.Map<RefreshToken>( refreshToken));
            RefreshTokenModel newRefreshToken = new RefreshTokenModel
            {
                Token = _jwt.GenerateRefreshToken(),
                Expires = DateTime.Now.AddDays(7),
                CreatedFromIp = ip,
                IsRevoked = false,
                UserId = user.Id
            };
            await _refreshTokenRepo.SaveTokenAsync(user.Id, _mapper.Map<RefreshToken>(newRefreshToken));
            return new AuthenticationResponse
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token,
            };
        }
    }
}
