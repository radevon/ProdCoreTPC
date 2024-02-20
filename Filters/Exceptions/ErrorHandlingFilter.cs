using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProdCoreTPC.Filters.Exceptions
{
    public class ErrorHandlingFilter : ExceptionFilterAttribute
    {
       

       
        public override void OnException(ExceptionContext context)
        {
           
            var exception = context.Exception;
            // add logging exception here!!!
            Debug.WriteLine("ошибка в проекте:"+exception.Message);
            context.ExceptionHandled = false; //not stop exception 
        }
    }

}