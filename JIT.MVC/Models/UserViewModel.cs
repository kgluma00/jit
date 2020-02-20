using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JIT.MVC.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        [Display(Name = "Last name")]
        public string Email { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsAdmin { get; set; }
        public bool isAuthenticated { get; set; }

        public ProjectViewModel Project { get; set; }
    }
}
