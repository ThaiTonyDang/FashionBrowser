using Fashion.Browser.VpayServices;
using FashionBrowser.Domain.Config;
using FashionBrowser.Domain.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
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
			services.AddScoped<IUrlServices, UrlServices>();
			services.AddScoped<IOrderServices, OrderServices>();
			services.AddScoped<ICheckoutServices, CheckoutServices>();
			services.AddScoped<IUserServices, UserServices>();
			services.AddScoped<IMapServices, MapServices>();
			services.AddScoped<IFileServices, FileServices>();
			services.AddScoped<IVnpayServices, VnpayServices>();

            services.AddHttpContextAccessor();
            services.AddHttpClient();

			var tokenConfig = Configuration.GetSection("Token");
            services.Configure<TokenConfig>(tokenConfig);
            services.Configure<APIConfig>(Configuration.GetSection("Api"));
            services.Configure<PageConfig>(Configuration.GetSection("Page"));
            services.Configure<VnpayConfig>(Configuration.GetSection("Vnpay"));
            services.Configure<OrderCallbackConfig>(Configuration.GetSection("OrderCallback"));

            var expiredTime = tokenConfig.Get<TokenConfig>().Expired;
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie(options => {
				options.LoginPath = "/users/login";
				options.ExpireTimeSpan = TimeSpan.FromMinutes(expiredTime);
                options.Cookie.Name = "Browser.Authentication";
            });

            services.AddDistributedMemoryCache();
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

			app.UseRouting();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
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
