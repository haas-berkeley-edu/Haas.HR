using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Haas.HR.Models;

namespace Haas.HR.Data
{
    public class HRDbContext : DbContext
    {
        public HRDbContext(DbContextOptions<HRDbContext> options) : base(options)
        {
        }

        public DbSet<PingboardEmployee> PingboardEmployees { get; set; }
        public DbSet<UCPathEmployee> UCPathEmployees { get; set; }

        public DbSet<HRDataSourceConnectionSettings> HRDataSourceConnectionSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PingboardEmployee>().ToTable("PingboardEmployee");
            modelBuilder.Entity<UCPathEmployee>().ToTable("UCPathEmployee");
            modelBuilder.Entity<HRDataSourceConnectionSettings>().ToTable("HRDataSourceConnectionSettings");
        }
    }
}
