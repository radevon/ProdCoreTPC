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
using Microsoft.Extensions.Configuration;

namespace ProdCoreTPC.Controllers
{
    [Authorize(Roles = "administrator")]
    public class ManageProfileController : Controller
    {

        
        private RoleManager<IdentityRole> roleManager;

        private UserManager<ApplicationUser> userManager;

        private IConfiguration configuration;


        public ManageProfileController(RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.roleManager = _roleManager;
            this.userManager = userManager;
            this.configuration = configuration;
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
        [HttpGet]
        public async Task<IActionResult> DeleteProfile(string userId)
        {
            ApplicationUser userToDeleted = await userManager.FindByIdAsync(userId);
            if (userToDeleted == null)
            {
                return Content($"Пользователь не найден");
            }

            return PartialView("_DeleteProfile", userToDeleted);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProfileConfirm(string userId)
        {
            ApplicationUser userToDeleted = await userManager.FindByIdAsync(userId);
            if (userToDeleted == null)
            {
                return Content($"Пользователь не найден");
            }

            if (User.Identity.Name == userToDeleted.UserName)
            {
                return Content($"Собственный аккаунт невозможно удалить");
            }

            string adminUser = configuration.GetValue<string>("AdministratorAccount:Login");
            if (userToDeleted.UserName==adminUser)
            {
                return Content($"Аккаунт администратора запрещено удалять");
            }

            IdentityResult result = await userManager.DeleteAsync(userToDeleted);

            if (result.Succeeded)
            {
                return Content($"Пользователь '{userToDeleted.UserName}' успешно удалён! Страница будет перезагружена" + ScriptConstants.RELOAD_SCRIPT);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return PartialView("_DeleteProfile", userToDeleted);
            }
        }

        /// <summary>
        /// Редактирование списка ролей у пользователя с userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IActionResult> ChangeProfileRoles(string userId)
        {
            ApplicationUser user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Content($"Пользователь не найден");
            }

            List<string> allRoles = roleManager.Roles.Select(x => x.Name).OrderBy(x => x).ToList();
            List<string> userRoles = (List<string>) await userManager.GetRolesAsync(user);
            Dictionary<string, bool> haveRoles = new Dictionary<string, bool>();
            foreach(string role in allRoles)
            {
                haveRoles.Add(role, userRoles.Contains(role));
            }

            UserRolesModel model = new UserRolesModel()
            {
                UserId = user.Id,
                UserName=user.UserName,
                Roles = haveRoles
            };            

            return PartialView("_UserRoles",model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRolesSave(UserRolesModel userRoleModel)
        {
            ApplicationUser user = await userManager.FindByIdAsync(userRoleModel.UserId);
            if (user == null)
            {
                return Content($"Пользователь {userRoleModel.UserName} не найден");
            }
            List<IdentityResult> operationResults = new List<IdentityResult>();
            foreach ( var (role,isHave) in userRoleModel.Roles)
            {
                bool isInRole = await userManager.IsInRoleAsync(user, role);
                
                //Пользователь имеет роль но не должен ее иметь - удаляю
                if (isInRole&&!isHave)
                {
                    // проверка чтобы нельзя было удалить собственную роль администратора
                    if (user.UserName == configuration.GetValue<string>("AdministratorAccount:Login") && role == "administrator")
                        continue;
                    operationResults.Add(await userManager.RemoveFromRoleAsync(user, role));
                }else if(!isInRole&&isHave) // пользователь не имеет роли, но должен - добавляю
                {
                    operationResults.Add(await userManager.AddToRoleAsync(user, role));
                }  
            }
            if (operationResults.Count == 0)
            {
                return Content("Изменений не зафиксировано!");
            }

            if (operationResults.TrueForAll(x => x.Succeeded))
            {
                return Content("Переназначение ролей прошло успешно! Страница будет перезагружена" + ScriptConstants.RELOAD_SCRIPT);
            }else
            {
                foreach(var res in operationResults.Where(x => !x.Succeeded))
                {
                    foreach (var error in res.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return PartialView("_UserRoles", userRoleModel);
            }
            
        }


        [HttpGet]
        public async Task<IActionResult> ChangePass(string userId)
        {
            ApplicationUser user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Content($"Пользователь не найден");
            }

            string resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

            PasswordResetModel resetModel = new PasswordResetModel()
            {
                UserId = userId,
                UserName = user.UserName,
                NewPassword = String.Empty,
                PasswordResetToken = resetToken
            };
           
            return PartialView("_ChangePassword",resetModel);

        }

        [HttpPost]
        public async Task<IActionResult> ChangePass(PasswordResetModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_ChangePassword", model);

            ApplicationUser user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return Content($"Пользователь не найден");
            }

            IdentityResult result = await userManager.ResetPasswordAsync(user, model.PasswordResetToken, model.NewPassword);


            if (result.Succeeded)
            {
                return Content($"Пароль у пользователя '{model.UserName}' успешно изменен! Страница будет перезагружена" + ScriptConstants.RELOAD_SCRIPT);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return PartialView("_ChangePassword", model);
            }
        }

    }
}
