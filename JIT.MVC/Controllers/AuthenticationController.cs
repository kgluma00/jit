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
        private readonly IJitService _jitService;
        private readonly IOptions<EmailOptions> _emailOptions;

        public AuthenticationController(IEmailService emailService, IJitService jitService, IOptions<EmailOptions> emailOptions)
        {
            _emailService = emailService;
            _jitService = jitService;
            _emailOptions = emailOptions;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateUser(int id)
        {
           var sendEmail = await _emailService.SendEmail(_emailOptions.Value.SendGridApiKey,id);
            return Ok(sendEmail);
        }

        [HttpGet]
        [Route("auth/{id}")]
        public async Task<IActionResult> AuthenticateUserByRequest(int id)
        {
            var isAuth = await _jitService.AuthenticateUser(id);

            if (isAuth)
                return RedirectToAction("Index", "Home");

            return RedirectToAction("NotFound", "Home");
        }
    }
}