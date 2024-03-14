using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebSockets.Internal;
using ProdCoreTPC.Code.Identity;
using ProdCoreTPC.Code.Interfaces;
using ProdCoreTPC.Code.Repositories;
using ProdCoreTPC.Models;
using ProdCoreTPC.Code;
using System.Data;


namespace ProdCoreTPC.Controllers
{
    [Authorize(Roles = "administrator")]
    public class ManageProfileController : Controller
    {

        private IRepository<ApplicationUser, String> userRepo;
        //private IRepository<IdentityRole, String> roleRepo;
        private RoleManager<IdentityRole> roleManager;

        private UserManager<ApplicationUser> userManager;



        public ManageProfileController(IRepository<ApplicationUser, String> _userRepo, IRepository<IdentityRole, String> _roleRepo, RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> userManager)
        {
            this.userRepo = _userRepo;
            //this.roleRepo = _roleRepo;
            this.roleManager = _roleManager;
            this.userManager = userManager;
        }

        public IActionResult UserList()
        {
            
            var users = userManager.Users.OrderBy(x=>x.UserName).ToList();
            return View(users);
        }

        public IActionResult RoleList()
        {
            
            var roles = roleManager.Roles.OrderBy(x => x.Name).ToList();
            return View(roles);
        }


       
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role == null)
                role = new IdentityRole() { Id = "0", Name = "" };
            return PartialView("_EditRole",new RoleModel { Id = role.Id, Name = role.Name });
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(RoleModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_EditRole", model);
            IdentityRole currentRole = await roleManager.FindByIdAsync(model.Id);
            if (model.Id == "0") // вставка новой записи
            {
                if (currentRole != null)
                {
                    ModelState.AddModelError(string.Empty, $"Роль с названием {model.Name} уже существует!");
                    return PartialView("_EditRole", model);
                }
                else
                {
                   
                        IdentityResult result = await roleManager.CreateAsync(new IdentityRole(model.Name));
                        if (result.Succeeded)
                        {
                            return Content($"Роль <b>'{model.Name}'</b> успешно добавлена! Страница будет перезагружена"+ ScriptConstants.RELOAD_SCRIPT) ;
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            return PartialView("_EditRole", model);
                        }
                    
                   
                }
            }
            else // обновление записи
            {
                if(currentRole!=null)
                {
                    if (currentRole.Name == "administrator")
                    {
                        return Content($"Переименование текущей роли запрещено!");
                    }
                    currentRole.Name = model.Name;
                    IdentityResult result = await roleManager.UpdateAsync(currentRole);
                    if (result.Succeeded)
                    {
                        return Content($"Роль успешно обновлена на '{model.Name}'! Страница будет перезагружена" + ScriptConstants.RELOAD_SCRIPT);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return PartialView("_EditRole", model);
                    }
                }else
                {
                    ModelState.AddModelError(string.Empty, "Обновляемая запись не найдена!");
                    return PartialView("_EditRole", model);
                }
                
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            IdentityRole role = await roleManager.FindByIdAsync(roleId);
            if (role == null) {
                return Content($"Роль не найдена");
            }
            
            return PartialView("_DeleteRole",role);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRoleConfirm(string roleId)
        {
            IdentityRole currentRole = await roleManager.FindByIdAsync(roleId);
            if (currentRole == null)
            {
                return Content($"Роль не найдена");
            }
            if (currentRole.Name == "administrator")
            {
                return Content($"Удаление текущей роли запрещено!");
            }
            IdentityResult result = await roleManager.DeleteAsync(currentRole);
            if (result.Succeeded)
            {
                return Content($"Роль <b>'{currentRole.Name}'</b> успешно удалена! Страница будет перезагружена" + ScriptConstants.RELOAD_SCRIPT);
            }
            else
            {
               return Content($"При удалении возникли следующие ошибки: <ul>"+String.Join("",result.Errors.Select(x=>"<li>"+x.Description+"</li>").ToArray())+"</ul>");
            }
        }

        [HttpGet]
        public IActionResult AddProfile()
        {
            ProfileModel newProfile = new ProfileModel();
            return PartialView("_AddProfile",newProfile);
        }

        [HttpPost]
        public async Task<IActionResult> AddProfile(ProfileModel newProfile)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_AddProfile", newProfile);
            }
            ApplicationUser user = await userManager.FindByNameAsync(newProfile.UserName);
            if (user != null)
            {
                ModelState.AddModelError("","Пользователь с таким именем (логином) уже существует. Придумайте другой логин!");
                return PartialView("_AddProfile", newProfile);
            }
            IdentityResult result = await userManager.CreateAsync(new ApplicationUser() { UserName = newProfile.UserName, Email = newProfile.Email, PhoneNumber = newProfile.PhoneNumber }, newProfile.Password);

            if (result.Succeeded)
            {
                return Content($"Пользователь '{newProfile.UserName}' успешно создан! Страница будет перезагружена" + ScriptConstants.RELOAD_SCRIPT);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return PartialView("_AddProfile", newProfile);
            }

        }

        
    }
}
