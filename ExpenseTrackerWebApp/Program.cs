using ExpenseTrackerWebApp.Data;
using ExpenseTrackerWebApp.Interfaces;
using ExpenseTrackerWebApp.Models;
using ExpenseTrackerWebApp.Repositories;
using ExpenseTrackerWebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerWebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                //SIGN IN CONFIG
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;

                //PASSWORD CONFIG
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<AuthService>();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
            builder.Services.AddScoped<ExpenseService>();

			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Auth/Login";    
				options.LogoutPath = "/Auth/LogOut";
                options.AccessDeniedPath = "/Auth/AccesDenied";
			});


			var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{Id?}");

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await IdentityDataSeeder.SeedRoles(services);
            }

            app.Run();
        }
    }
}
