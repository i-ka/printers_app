using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Printer> Printers { get; set; }
        public DbSet<Place> Offices { get; set; }
        public DbSet<Cartridge> Cartridges { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Cartridge>().HasOne(c => c.Printer).WithOne(p => p.Cartidge);
            builder.Entity<Cartridge>().HasOne(c => c.Place).WithMany(o => o.Cartidges);
            builder.Entity<Cartridge>().HasIndex(c => c.InventoryNumber).IsUnique();

            builder.Entity<Printer>().HasOne(p => p.Office).WithMany(o => o.Printers);

            builder.Entity<Place>().HasOne(o => o.City).WithMany(c => c.Offices);

            builder.Entity<ApplicationUser>().HasOne(u => u.Place).WithMany(p => p.Users);

            base.OnModelCreating(builder);
        }
    }
}
