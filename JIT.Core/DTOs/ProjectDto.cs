using System;
using System.Collections.Generic;
using System.Text;

namespace JIT.Core.DTOs
{
   public class ProjectDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ProjectName { get; set; }
        public DateTime WorkingDate { get; set; }
        public double WorkingHours { get; set; }
        public ICollection<UserDto> User { get; set; }
    }
}
