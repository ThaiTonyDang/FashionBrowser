using FashionBrowser.Domain.Config;
using FashionBrowser.Domain.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Fashion.Browser
{
    public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.AddScoped<IProductServices, ProductServices>();
			services.AddScoped<ICategoryServices, CategoryServices>();
			services.AddScoped<ICartServices, CartServices>();
			services.AddScoped<IUrlService, UrlService>();
			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<IOrderDetailService, OrderDetailService>();
			services.AddScoped<ICustomerService, CustomerService>();
			services.AddScoped<ICheckoutService, CheckoutService>();
			services.AddScoped<IUserService, UserService>();

            services.AddHttpContextAccessor();
            services.AddHttpClient();

            services.Configure<APIConfig>(Configuration.GetSection("Api"));
            services.Configure<TokenConfig>(Configuration.GetSection("Token"));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/users/login";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
            });

            services.AddDistributedMemoryCache();

			services.AddSession(cfg => {
				cfg.Cookie.Name = "productData";
				cfg.IdleTimeout = new TimeSpan(24, 0, 0);
			});

            services.AddSession(cfg => {
                cfg.IdleTimeout = TimeSpan.FromDays(1);
            });
        }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();

			app.UseSession();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
