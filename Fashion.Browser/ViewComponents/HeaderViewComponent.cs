using FashionBrowser.Domain.Services;
using FashionBrowser.Domain.ViewModels;
using FashionBrowser.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fashion.Browser.ViewComponents
{
    public class HeaderComponentViewModel
    {
        public int CartItemCount { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string FullName { get; set; }
        public CategoryItemViewModel CategoryMen { get; set; }
        public CategoryItemViewModel CategoryWomen { get; set; }
        public CategoryItemViewModel CategoryKid { get; set; }
    }

    public class HeaderViewComponent : ViewComponent
    {
        private readonly ICartServices _cartservice;
        private readonly IUserServices _userService;
        private readonly ICategoryServices _categoryservice;
        public HeaderViewComponent(ICartServices cartServices, IUserServices userService, ICategoryServices categoryServices)
        {
            this._cartservice = cartServices;
            this._userService = userService;
            this._categoryservice = categoryServices;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModel = new HeaderComponentViewModel();

            if (UserClaimsPrincipal.Identity.IsAuthenticated)
            {
                var token = UserClaimsPrincipal.FindFirstValue(JwtClaimType.Token);
                var userTask = Task.Run(() => _userService.GetUserAsync(token));
                var cartTask = Task.Run(() => _cartservice.GetCartViewModel(token));
                var tasks = new Task[] { userTask, cartTask };
                await Task.WhenAll(tasks);
                var user = await userTask;
                var cart = await cartTask;

                viewModel.FullName = $"{user.FirstName} {user.LastName}";
                viewModel.Avatar = user.ImageUrl;
                viewModel.Email = UserClaimsPrincipal.FindFirstValue(ClaimTypes.Email);
                viewModel.CartItemCount = cart.ListCartItem.Count;

            }

            var menFashion = $"/products/{CATEGORY.MEN_FASHION}";
            var womenFashion = $"/products/{CATEGORY.WOMEN_FASHION}";
            var kidFashion = $"/products/{CATEGORY.KID_FASHION}";

            var categoryMenTask = Task.Run(() => _categoryservice.GetCategoryByName(CATEGORY.MEN_FASHION));
            var categoryWomenTask = Task.Run(() => _categoryservice.GetCategoryByName(CATEGORY.WOMEN_FASHION));
            var categoryKidTask = Task.Run(() => _categoryservice.GetCategoryByName(CATEGORY.KID_FASHION));

            var listTask = new Task[]
            {
                categoryMenTask,
                categoryWomenTask,
                categoryKidTask
            };

            await Task.WhenAll(listTask);

            var categoryMen = await categoryMenTask;
            var categoryWomen = await categoryWomenTask;
            var categoryKid = await categoryKidTask;

            viewModel.CategoryMen = categoryMen;
            viewModel.CategoryWomen = categoryWomen;
            viewModel.CategoryKid = categoryKid;

            return View(viewModel);
        }
    }
}
