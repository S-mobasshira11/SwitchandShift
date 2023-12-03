using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Switch_and_Shift.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Switch_and_Shift.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<USERS> USERS { get; set; }
        public DbSet<PRODUCTS> PRODUCTS { get; set; }
        public DbSet<ADMIN> ADMINs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<USERS>().ToTable("USERS");
        }

        public DbSet<Switch_and_Shift.Models.ADMIN> ADMIN { get; set; }

        public DbSet<Switch_and_Shift.Models.USERREVIEW> USERREVIEW { get; set; }




       

    }
}
