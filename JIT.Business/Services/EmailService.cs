using JIT.Business.Interfaces;
using JIT.Core.DTOs;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JIT.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly IJitService _jitService;

        public EmailService(IJitService jitService)
        {
            _jitService = jitService;
        }

        public async Task<bool> SendEmail(UserDto? user, string secretKey,int? id)
        {
            if (user == null)
            {
                user = await _jitService.GetUserById(id.Value);
            }
            var apiKey = secretKey;
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("info@jit.com", "Admin user");
            var subject = "Welcome to Just in Time";
            var to = new EmailAddress(user.Email, user.Name);
            var plainTextContent = "Please click the link to activate your account";
            var htmlContent = "Please click the <a href=''>link</a> below to activate your account";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted) return true;

            return false;
        }


    }
}
