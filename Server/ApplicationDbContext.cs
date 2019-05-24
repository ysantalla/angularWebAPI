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

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Agency> Agencies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}