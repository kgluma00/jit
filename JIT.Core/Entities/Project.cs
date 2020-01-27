using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JIT.Core.Entities
{
    public class Project
    {

        public int Id { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public DateTime WorkingDate { get; set; }
        [Required]
        public double WorkingHours { get; set; }
        public ICollection<User> User { get; set; }
    }
}
