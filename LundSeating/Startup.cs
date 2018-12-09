using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LundSeating.Data;
using LundSeating.Models;
using LundSeating.Services;

namespace LundSeating
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                //Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 9;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredUniqueChars = 6;

                // lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(1);

                // If the LoginPath isn't set, ASP.NET Core defaults 
                // the path to /Account/Login.
                options.LoginPath = "/Account/Login";

                // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                // the path to /Account/AccessDenied.
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;

            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext context,IServiceProvider serviceProvider
            )
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

      

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

           CreateRoles(serviceProvider).Wait();
            DbInitalizer.Initialize(context);
        }
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var rolemanager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] rolenames = { "ADMIN", "VIEWER", "MANAGER" };
            IdentityResult roleresult;
            foreach (var rolename in rolenames)
            {
                var roleexist = await rolemanager.RoleExistsAsync(rolename);
                if (!roleexist)
                {
                    roleresult = await rolemanager.CreateAsync(new IdentityRole(rolename));
                }
            }

            var _user = await UserManager.FindByEmailAsync("admin@temp.com");
            if (_user == null)
            {
                var poweruser = new ApplicationUser
                {
                    UserName = "admin@temp.com",
                    Email = "admin@temp.com"
                    
                    
                };

                String adminpassword = "Password1***";

                var createuser = await UserManager.CreateAsync(poweruser, adminpassword);
                if (createuser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(poweruser, "ADMIN");
                }
            }

        }

    }
}
