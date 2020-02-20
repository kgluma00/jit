using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JIT.Business.Interfaces;
using JIT.MVC.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JIT.MVC.Controllers
{
    [Authorize]
    public class EmailController : Controller
    {
        private readonly IOptions<EmailOptions> _emailOptions;
        private readonly IEmailService _emailService;

        public EmailController(IOptions<EmailOptions> emailOptions, IEmailService emailService)
        {
            _emailOptions = emailOptions;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SendEmail()
        {
            //await _emailService.SendEmail(, _emailOptions.Value.SendGridApiKey);
            return Ok();
        }
    }
}