using TheFinalProject.DAL;
using Microsoft.EntityFrameworkCore;

namespace TheFinalProject
{
    public static class RegisterService
    {
        public static void Register(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllersWithViews();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            }
            );

            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(1);
            });



        }
    }
}
