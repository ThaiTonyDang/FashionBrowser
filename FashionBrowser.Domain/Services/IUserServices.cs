using FashionBrowser.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FashionBrowser.Domain.Services
{
    public interface IUserServices
	{
		public Task<Tuple<bool, string>> RegisterUserAsync(RegisterItemViewModel registerUser);
		public Task<Tuple<ClaimsPrincipal, bool, string>> LoginAsync(LoginItemViewModel loginUser);
        public IEnumerable<Claim> GetClaims(ClaimsPrincipal claimsPrincipal, string token);
        public Task<Tuple<bool, string>> UpdateUserAsync(UserItemViewModel userItemViewModel, string token);
        public Task<Tuple<bool, string>> UpdateUserAvatarAsync(UserItemViewModel userItemViewModel, string token);
        public Task<UserItemViewModel> GetUserAsync(string token);
        public Task<Tuple<bool, string>> ChangePassword(PasswordItemViewModel passwordItemViewModel, string token);
    }
}
