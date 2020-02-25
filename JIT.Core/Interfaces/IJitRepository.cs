using JIT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JIT.Core.Interfaces
{
    public interface IJitRepository
    {
        Task<ICollection<User>> GetAllUsers();
        Task<User> GetUserById(int id, bool includeWorkingHours = false);
        Task<User> GetUserByUsername(string username);
        Task<User> Register(User user);
        Task<bool> UserExists(string username);
        Task<ICollection<Project>> GetAllProjectsByUserId(int userId);
        Task<ICollection<Project>> GetAllProjectsBetweenDates(int userId, DateTime startDate, DateTime endDate);
        Task<bool> DeleteProject(int id);
        void Update(User user);
        void Delete(User user);
        Task<Project> SaveNewProject(Project project);
    }
}
