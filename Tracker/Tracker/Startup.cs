using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Tracker.Hubs;
using Tracker.Models;
using Tracker.Utils;


namespace Tracker
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
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IPasswordValidator<User>, PasswordValidator>(serv => new PasswordValidator(serv.GetRequiredService<IStringLocalizer<PasswordValidator>>(), 6));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("ru"),
                    new CultureInfo("en")
                };

                options.DefaultRequestCulture = new RequestCulture("ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chat");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "PrivateAccount",
                    template: "{controller=Home}/{action=Private}");
                routes.MapRoute(
                    name: "FamilyAccount",
                    template: "{controller=Home}/{action=Family}");
                routes.MapRoute(
                    name: "CommonAccounts",
                    template: "{controller=Home}/{action=Common}");
                routes.MapRoute(
                    name: "Chat",
                    template: "{controller=Chat}/{action=Index}/{id}/{isOpenModal:bool}/{modalText?}");
                routes.MapRoute(
                    name: "AdminRoom",
                    template: "{controller=Roles}/{action=Admin}");
                routes.MapRoute(
                    name: "ModeratorRoom",
                    template: "{controller=Roles}/{action=Moderator}");
                routes.MapRoute(
                    name: "ChangeAdminRole",
                    template: "{controller=Roles}/{action=AdminRole}/{id}");
                routes.MapRoute(
                    name: "ChangeModeratorRole",
                    template: "{controller=Roles}/{action=ModeratorRole}/{id}");
                routes.MapRoute(
                    name: "ChangeBlockedUserRole",
                    template: "{controller=Roles}/{action=BlockedUserRole}/{id}");
                routes.MapRoute(
                    name: "SignUp",
                    template: "{controller=Account}/{action=SignUp}");
                routes.MapRoute(
                    name: "LogIn",
                    template: "{controller=Account}/{action=LogIn}");
                routes.MapRoute(
                    name: "LogOff",
                    template: "{controller=Account}/{action=LogOff}");
            });
        }
    }
}
