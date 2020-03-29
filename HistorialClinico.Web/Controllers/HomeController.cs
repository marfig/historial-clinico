#region Using

using HistorialClinico.Common.Exceptions;
using HistorialClinico.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

#endregion

namespace HistorialClinico.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        public HomeController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(PacienteController));
        }
        public IActionResult Index()
        {
            return Redirect("/camapaciente");
        }

        public IActionResult Error(string error)
        {
            ErrorViewModel model = new ErrorViewModel()
            {
                Message = error
            };

            return View(model);
        }
    }
}
