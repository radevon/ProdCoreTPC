using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProdCoreTPC.Code.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProdCoreTPC.ViewComponents
{
    public class UserRoles: ViewComponent
    {
        private UserManager<ApplicationUser> userManager;

        public UserRoles(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            ApplicationUser user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Content("");
            List<string> userRoles = (List<string>)await userManager.GetRolesAsync(user);
            return View("_UserRoles", userRoles);
        }
    }
}
