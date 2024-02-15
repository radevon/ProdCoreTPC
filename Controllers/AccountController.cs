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
        public IActionResult Login(UserLogin user)
        {
            ModelState.AddModelError("", "Неверный логин или пароль");
            if (!ModelState.IsValid)
                return View(user);

            // проверка пользователя

            return RedirectToAction("Index", "Start");
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }

        
    }
}
