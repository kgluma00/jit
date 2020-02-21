using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JIT.Business.Interfaces;
using JIT.MVC.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JIT.MVC.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IOptions<EmailOptions> _emailOptions;

        public AuthenticationController(IEmailService emailService, IOptions<EmailOptions> emailOptions)
        {
            _emailService = emailService;
            _emailOptions = emailOptions;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateUser(int id)
        {
            await _emailService.SendEmail(null,_emailOptions.Value.SendGridApiKey,id);
            return View();
        }
    }
}