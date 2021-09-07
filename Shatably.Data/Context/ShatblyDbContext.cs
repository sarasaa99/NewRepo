using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shatably.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shatably.Data.Context
{
    public class ShatblyDbContext : IdentityDbContext<ApplicationUser>
    {
        public ShatblyDbContext(DbContextOptions<ShatblyDbContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Reviews> Reviewss { get; set; }
        public DbSet<Appartment> Appartments { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
       .HasMany(c => c.Companies)
       .WithOne(e => e.ApplicationUser);

            modelBuilder.Entity<Company>()
       .HasOne(e => e.ApplicationUser)
       .WithMany(c => c.Companies);

            modelBuilder.Entity<ApplicationUser>()
      .HasMany(c => c.Appartments)
      .WithOne(e => e.ApplicationUser);

            modelBuilder.Entity<Appartment>()
       .HasOne(e => e.ApplicationUser)
       .WithMany(c => c.Appartments);

            modelBuilder.Entity<ApplicationUser>()
      .HasMany(c => c.UserReview)
      .WithOne(e => e.UserID);

            modelBuilder.Entity<Reviews>()
       .HasOne(e => e.UserID)
       .WithMany(c => c.UserReview);

            modelBuilder.Entity<ApplicationUser>()
     .HasMany(c => c.CompanyReview)
     .WithOne(e => e.CompanyIDUser);

            modelBuilder.Entity<Reviews>()
       .HasOne(e => e.CompanyIDUser)
       .WithMany(c => c.CompanyReview);

            modelBuilder.Entity<Company>()
      .HasMany(c => c.Galleries)
      .WithOne(e => e.Company);

            modelBuilder.Entity<Gallery>()
       .HasOne(e => e.Company)
       .WithMany(c => c.Galleries);
            modelBuilder.Entity<Gallery>()
        .HasKey(g => new { g.GalleryID, g.CompanyId });
            modelBuilder.Entity<Company>()
      .HasMany(c => c.Reviews)
      .WithOne(e => e.Company);

            modelBuilder.Entity<Reviews>()
       .HasOne(e => e.Company)
       .WithMany(c => c.Reviews);

            modelBuilder.Entity<Company>().Property("CompanyName").IsRequired();
            modelBuilder.Entity<Company>().Property("Address").IsRequired();
            modelBuilder.Entity<Company>().Property("City").IsRequired();
            modelBuilder.Entity<Company>().Property("Country").IsRequired();
            modelBuilder.Entity<Company>().Property("Phone1").IsRequired();
        }

       

    }
}
