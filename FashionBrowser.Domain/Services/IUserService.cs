using FashionBrowser.Domain.Dto;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Domain.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FashionBrowser.Domain.Services
{
    public interface IUserService
	{
		public Task<ResultDto> RegisterUserAsync(RegisterItemViewModel registerUser);
		public Task<ResultDto> LoginAsync(UserLoginViewModel loginUser);
        public IEnumerable<Claim> GetClaims(string token);
        public Task<ResultDto> UpdateUserProfileAsync(UserViewModel userItemViewModel, string token);
        public Task<ResultDto> UpdateUserAvatarAsync(MultipartFormDataContent file, string token);
        public Task<ResultDto> GetUserProfileAsync(string token);
        public Task<ResultDto> ChangePassword(UserPasswordViewModel passwordItemViewModel, string token);
    }
}
