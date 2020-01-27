using JIT.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JIT.Repository
{
    public class JitContext : DbContext
    {
        public JitContext(DbContextOptions<JitContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
