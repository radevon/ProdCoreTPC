using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProdCoreTPC.Code.Identity;
using ProdCoreTPC.Code.Interfaces;
using ProdCoreTPC.Code.Repositories;
using ProdCoreTPC.Models;

namespace ProdCoreTPC.Controllers
{
    [Authorize(Roles = "administrator")]
    public class ManageProfileController : Controller
    {

        private IRepository<ApplicationUser, String> userRepo;
        private IRepository<IdentityRole, String> roleRepo;

        public ManageProfileController(IRepository<ApplicationUser, String> _userRepo, IRepository<IdentityRole, String> _roleRepo)
        {
            this.userRepo = _userRepo;
            this.roleRepo = _roleRepo;
        }

        public IActionResult UserList()
        {
            
            var users = userRepo.GetAll().OrderBy(x=>x.UserName);
            return View(users);
        }

        public IActionResult RoleList()
        {
            var roles = roleRepo.GetAll().OrderBy(x => x.Name);
            return View(roles);
        }

        [HttpGet]
        public IActionResult EditRole(string id)
        {
            IdentityRole role = roleRepo.Get(id);
            if (role == null)
                role = new IdentityRole() { Id = "0", Name = "" };
            return PartialView("_EditRole",new RoleModel { Id = role.Id, Name = role.Name });
        }

    }
}
