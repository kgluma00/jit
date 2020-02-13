using JIT.Core.DTOs;
using JIT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JIT.Business.Interfaces
{
   public interface IJitService
    {
        Task<ICollection<UserDto>> GetAllUsers();
        Task<ICollection<ProjectDto>> GetAllProjectsByUserId(int userId);
        Task<ICollection<ProjectDto>> GetAllProjectsBetweenDates(int userId, DateTime startDate, DateTime endDate);
        Task<UserDto> GetUserById(int id, bool includeWorkingHours = false);
        Task<UserDto> Register(UserDto user);
        Task<bool> UserExists(UserDto user);
        Task<UserDto> Login(UserDto user);
        Task<User> GetUserByUsername(UserDto user);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        public Task<Project> SaveNewProject(ProjectDto project);
        //void Create(UserDto user);
        void Update(UserDto user);
        void Delete(UserDto user);
    }
}
