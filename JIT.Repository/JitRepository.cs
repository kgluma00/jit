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
        }

        public async Task<ICollection<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserById(int id, bool includeWorkingHours = false)
        {
            if (includeWorkingHours)
            {
                return await _context.Users.Where(i => i.Id == id).Include(p => p.Project).FirstOrDefaultAsync();
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

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(u => u.Username == username)) return true;

            return false;
        }
    }
}
