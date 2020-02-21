using JIT.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JIT.Business.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmail(UserDto? user, string secretKey, int? id);
    }
}
