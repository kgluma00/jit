using JIT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIT.Core.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsAuthenticated { get; set; }
        public ICollection<ProjectDto> Projects { get; set; }
    }
}
