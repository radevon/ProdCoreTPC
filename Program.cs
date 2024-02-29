using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProdCoreTPC.Code.Logging;
using ProdCoreTPC.Code.Identity;

namespace ProdCoreTPC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = CreateWebHostBuilder(args);

            builder.ConfigureLogging(config =>
            {
                config.ClearProviders();
                config.AddProvider(new JsonFileLoggerProvider("logs"));
            });

            var host = builder.Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var configuration = services.GetRequiredService<IConfiguration>();
                    AuthInitializer initializer = new AuthInitializer(configuration);
                    await initializer.InitializeAsync(userManager,rolesManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Ошибка при инициализации базы данных авторизации в методе Main");
                }
            }

     


            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
