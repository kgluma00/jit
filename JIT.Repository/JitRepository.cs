using JIT.Core.Entities;
using JIT.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIT.Repository
{
    public class JitRepository : IJitRepository
    {
        private readonly JitContext _context;

        public JitRepository(JitContext context)
        {
            _context = context;
        }

        public void Delete(User user)
        {
            _context.Remove(user);
            _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteProject(int id)
        {
            var projectToDelete = _context.Projects.Where(i => i.Id == id).SingleOrDefault();
            _context.Projects.Remove(projectToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ICollection<Project>> GetAllProjectsBetweenDates(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Projects.Where(p => p.UserId == userId && p.WorkingDate >= startDate && p.WorkingDate <= endDate).ToListAsync();
        }

        public async Task<ICollection<Project>> GetAllProjectsByUserId(int userId)
        {
            return await _context.Projects.Where(i => i.UserId == userId).OrderByDescending(i => i.WorkingDate).ToListAsync();
        }

        //public  Task<ICollection<Project>> GetAllProjectsInRangeByUserId(ICollection<Project> projects,int pageNumber, int pageSize)
        //{
        //    int excludeRecords = (pageSize * pageNumber) - pageSize;

        //    return projects.Skip(excludeRecords).Take(pageSize);
        //}

        public async Task<ICollection<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Project> GetProjectById(int id)
        {
            return await _context.Projects.Where(i => i.Id == id).SingleOrDefaultAsync();
        }

        public async Task<User> GetUserById(int id, bool includeWorkingHours = false)
        {
            if (includeWorkingHours)
            {
                //return await _context.Users.Where(i => i.Id == id).Include(p => p.Project).FirstOrDefaultAsync();
            }

            return await _context.Users.Where(i => i.Id == id).FirstOrDefaultAsync();

        }

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await _context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
            return await _context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<User> Register(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Project> SaveNewProject(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public async Task<bool> UpdateProject(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(u => u.Username == username)) return true;

            return false;
        }
    }
}
