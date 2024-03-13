using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProdCoreTPC.Code.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProdCoreTPC.ViewComponents
{
    public class UsersInRole: ViewComponent
    {
        private UserManager<ApplicationUser> userManager;

        public UsersInRole(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string roleName)
        {
            List<ApplicationUser> users = (List<ApplicationUser>)await userManager.GetUsersInRoleAsync(roleName);
            return View("_UsersInRole", users);
        }
    }
}
