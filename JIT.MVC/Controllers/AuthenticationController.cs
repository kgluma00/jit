using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JIT.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JIT.MVC.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IJitService _jitService;

        public AuthenticationController(IJitService jitService)
        {
            _jitService = jitService;
        }

        [HttpPost]
        [Route("auth/{id}")]
        public async Task<IActionResult> AuthenticateUser(int id)
        {
            await _jitService.AuthenticateUser(id);
            return View();
        }
    }
}