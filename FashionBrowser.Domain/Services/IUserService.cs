using FashionBrowser.Domain.Dto;
using FashionBrowser.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FashionBrowser.Domain.Services
{
    public interface IUserService
	{
		public Task<ResultDto> RegisterUserAsync(RegisterItemViewModel registerUser);
		public Task<ResultDto> LoginAsync(LoginItemViewModel loginUser);
        public IEnumerable<Claim> GetClaims(string token);
        public Task<Tuple<bool, string>> UpdateUserAsync(UserItemViewModel userItemViewModel, string token);
        public Task<Tuple<bool, string>> UpdateUserAvatarAsync(UserItemViewModel userItemViewModel, string token);
        public Task<ResultDto> GetUserProfileAsync(string token);
        public Task<Tuple<bool, string>> ChangePassword(PasswordItemViewModel passwordItemViewModel, string token);
    }
}
