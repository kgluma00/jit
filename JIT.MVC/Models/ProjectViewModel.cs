using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JIT.MVC.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime WorkingDate { get; set; }
        public double WorkingHours { get; set; }
        public ICollection<UserViewModel> User { get; set; }
    }
}
