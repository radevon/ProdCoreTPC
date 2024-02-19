using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProdCoreTPC.Identity
{
    public class AuthInitializer
    {
        private IConfiguration _configuration;

        public AuthInitializer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task InitializeAsync(UserManager<ApplicationUser> _uMan, RoleManager<IdentityRole> _rMan)
        {
            try
            {
                string adminUser = _configuration.GetValue<string>("AdministratorAccount:Login");
                string adminPassword = _configuration.GetValue<string>("AdministratorAccount:Password"); ;
                if (await _rMan.FindByNameAsync("administrator") == null)
                {
                    await _rMan.CreateAsync(new IdentityRole("administrator"));
                }

                if (await _uMan.FindByNameAsync(adminUser) == null)
                {
                    ApplicationUser admin = new ApplicationUser { Email = "", UserName = adminUser };
                    IdentityResult result = await _uMan.CreateAsync(admin, adminPassword);
                    if (result.Succeeded)
                    {
                        await _uMan.AddToRoleAsync(admin, "administrator");
                    }
                }
            }catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
