using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProdCoreTPC.Controllers
{
    public class ErrorController : Controller
    {

        // срабатывает после перехвата исключения на проде по средством UseExceptionHandler мидлвара в Startup
        // к этому моменту исключение уже прошло обработку в фильтре исключений (в котором происходит логирование) поэтому здесь задача его только отобразить
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            var exception = context.Error;

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "В процессе работы приложения произошла непредвиденная ошибка!",
                Detail = exception.Message
            };
            return View(problemDetails);
        }
    }
}
