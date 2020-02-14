using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JIT.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using JIT.Core.DTOs;
using Microsoft.AspNetCore.Authentication;
using JIT.MVC.Helpers;

namespace JIT.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthenticateUser _authenticateUser;

        public HomeController(ILogger<HomeController> logger, AuthenticateUser authenticateUser)
        {
            _logger = logger;
            _authenticateUser = authenticateUser;
        }

        public IActionResult Index()
        {
            if (_authenticateUser.LoggedUserId == 0) return RedirectToAction("NotFound");
            return View();
        }

        [HttpGet]
        public IActionResult NotFound()
        {

            return View(_authenticateUser);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
