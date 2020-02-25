using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JIT.MVC.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Display(Name = "Project name")]
        public string ProjectName { get; set; }
        [Display(Name = "Working Date")]
        [DataType(DataType.Date)]
        public DateTime WorkingDate { get; set; }
        [Display(Name = "Working Hours")]
        public double WorkingHours { get; set; }
        public ICollection<UserViewModel> User { get; set; }
    }
}
