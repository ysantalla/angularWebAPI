using Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Server
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
            
        }

        public DbSet<Guest> Guets { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Currency> Currency { get; set; }

        public DbSet<Citizenship> Citizenships { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // builder.Entity<Country>()
            //     .HasOne(x => x.CreatorUser)
            //     .WithMany(x => x.Countries)
            //     .HasForeignKey(x => x.CreatorId);
            
            // builder.Entity<Citizenship>()
            //     .HasOne(x => x.CreatorUser);
            
            // builder.Entity<Currency>()
            //     .HasOne(x => x.CreatorUser);

            // builder.Entity<Guest>()
            //     .HasOne(x => x.CreatorUser);

        }
    }
}