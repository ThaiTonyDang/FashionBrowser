using ChelseaWeb.Domains.Services;
using FashionBrowser.Domain.Services;
using FashionBrowser.Infrastructure;
using FashionBrowser.Infrastructure.Config;
using FashionBrowser.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<IProductServices, ProductServices>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<ICategoryServices, CategoryServices>();
			services.AddScoped<ICartServices, CartService>();
			services.AddHttpContextAccessor();

			services.AddDistributedMemoryCache();

			services.AddSession(cfg => {
				cfg.Cookie.Name = "productData";
				cfg.IdleTimeout = new TimeSpan(24, 0, 0);
			});

			services.Configure<FileConfig>(Configuration.GetSection("FileConfig"));

			services.AddDbContext<AppDbContext>(x =>
											   x.UseSqlServer(Configuration.GetConnectionString("Ecommerce")));
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

			var fileConfig = Configuration.GetSection("FileConfig");

			if (fileConfig.Get<FileConfig>() != null)
			{
				var path = fileConfig.Get<FileConfig>().ImagePath;
				app.UseStaticFiles(new StaticFileOptions
				{
					FileProvider = new PhysicalFileProvider(path),
				});
			}

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
