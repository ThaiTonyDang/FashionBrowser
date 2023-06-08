using FashionBrowser.Domain.ViewModels;
using System.Security.Claims;

namespace FashionBrowser.Domain.Services
{
    public interface IUserService
	{
		public Task<Tuple<bool, string>> RegisterUserAsync(UserItemViewModel registerUser);
		public Task<Tuple<ClaimsPrincipal, bool, string>> VerifyUserAsync(LoginItemViewModel loginUser);
    }
}
