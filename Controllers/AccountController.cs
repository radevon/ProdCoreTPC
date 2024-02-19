using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ProdCoreTPC.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(UserLogin user)
        {
            
            if (!ModelState.IsValid)
                return View(user);

          
                // проверка пользователя
                var _user = await _userManager. FindByNameAsync(user.UserName);
                if(_user == null)
                {
                    ModelState.AddModelError("", "Не найден пользователь с таким именем!");
                    return View(user);
                }
                var result = await _signInManager.PasswordSignInAsync(_user, user.UserPassword, false, false);
                if (result.Succeeded)
                {

                }else
                {
                    ModelState.AddModelError("", "Неверный пароль!");
                    return View(user);
                }


           


            return RedirectToAction("Index", "Start");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
