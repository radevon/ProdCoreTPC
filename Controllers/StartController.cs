using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ProdCoreTPC.Controllers
{
    public class StartController : Controller
    {
        private ILogger _logger;

        public StartController(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger<StartController>() ;
        }

        public IActionResult Index()
        {

            
            _logger.LogInformation("Просто инфа {0} {1} {2}", 1, "dfff", DateTime.Now);
            return View();
        }
    }
}
