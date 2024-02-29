using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using ProdCoreTPC.Code.Interfaces;
using ProdCoreTPC.Code.Repositories;
using ProdCoreTPC.Filters.Exceptions;
using ProdCoreTPC.Code.Identity;

namespace ProdCoreTPC
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<AuthContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AuthConnection"))
           );

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.User.RequireUniqueEmail = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<AuthContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "ASP_CORE_AUTH";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.SlidingExpiration = true;
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";

                // добавляю обработчик автоматич. переадресации для неавторизованных запросов (отдельная обработка запросов из javascript) - не нужен редирект на страницу логина
                options.Events.OnRedirectToLogin += ctx =>
                {
                    if(ctx.Request.Headers.ContainsKey("X-Requested-With")&&ctx.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };

                // добавляю обработчик автоматич. переадресации для  запросов без необходимых прав (отдельная обработка запросов из javascript) - также не нужен редирект на страницу логина
                options.Events.OnRedirectToAccessDenied += ctx =>
                {
                    if (ctx.Request.Headers.ContainsKey("X-Requested-With") && ctx.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                };

            });
           


            services.AddMvc(options=>
            {
                options.Filters.Add(new ErrorHandlingFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient(typeof(IRepository<ApplicationUser, string>), typeof(AppUserRepository));
            services.AddTransient(typeof(IRepository<IdentityRole, string>), typeof(AppRoleRepository));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else // вывод пользователю сообщения об ошибке
            {
                app.UseExceptionHandler("/Error/Error");
            }

            app.UseStaticFiles();

             app.UseStaticFiles(new StaticFileOptions()
            {
                RequestPath = PathString.FromUriComponent("/libs"),
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "node_modules"))
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "dist")),
                RequestPath = "/dist"
            });

            app.UseAuthentication();    // подключение аутентификации

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Start}/{action=Index}/{id?}");
            });
                              

            app.UseStatusCodePages();
        }
    }
}
