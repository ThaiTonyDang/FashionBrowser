using Microsoft.EntityFrameworkCore;

namespace EcommercyWeb.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    }
}