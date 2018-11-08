using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Academy.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            var errorModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier
            };

            return this.View(errorModel);
        }
        [Route("error/404")]
        public IActionResult PageNotFound()
        {
            return this.View();
        }

        [Route("error/{code:int}")]
        public IActionResult InternalServerError()
        {
            return this.View();
        }
    }
}


