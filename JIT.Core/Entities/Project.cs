using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JIT.Core.Entities
{
    public class Project
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        public DateTime WorkingDate { get; set; }
        [Required]
        public double WorkingHours { get; set; }
        public User User { get; set; }
    }
}
