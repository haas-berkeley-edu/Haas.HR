using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Haas.HR.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Haas.HR.Data
{
    public class HRDbContext : DbContext
    {
        public HRDbContext(DbContextOptions<HRDbContext> options) : base(options)
        {
        }

        public DbSet<PingboardEmployee> PingboardEmployees { get; set; }
        public DbSet<UCPathEmployee> UCPathEmployees { get; set; }

        public DbSet<MasterEmployee> MasterEmployees { get; set; }

        public DbSet<SupervisorCalGroupEmployee> SupervisorCalGroupEmployees { get; set; }

        public DbSet<HRDataSourceConnectionSettings> HRDataSourceConnectionSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<PingboardEmployee>().ToTable("PingboardEmployee", "dbo");
            modelBuilder.Entity<UCPathEmployee>().ToTable("UCPathEmployee", "dbo");
            modelBuilder.Entity<MasterEmployee>().ToTable("MasterEmployee", "dbo");
            modelBuilder.Entity<SupervisorCalGroupEmployee>().ToTable("SupervisorCalGroupEmployee", "dbo");
            modelBuilder.Entity<HRDataSourceConnectionSettings>().ToTable("HRDataSourceConnectionSettings", "dbo");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=ssis17.haas.berkeley.edu;database=HR;trusted_connection=true;");
        }
    }
}
