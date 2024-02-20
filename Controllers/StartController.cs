using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProdCoreTPC.Controllers
{
    public class StartController : Controller
    {
        
        public IActionResult Index()
        {
            throw new Exception();
            return View();
        }
    }
}
