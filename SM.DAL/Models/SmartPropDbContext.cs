using System;
using System.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Protocols;
using SM.DAL.Models;


#nullable disable

namespace SM.DAL.Models
{
    public partial class SmartPropDbContext : DbContext
    {
        public SmartPropDbContext()
        {
        }

        public SmartPropDbContext(DbContextOptions<SmartPropDbContext> options): base(options)
        {
            
        }

        public DbSet<ImagesRealEstate> ImagesRealEstate { get; set; }
        public DbSet<RealEstate> RealEstates { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
              
               optionsBuilder.UseSqlServer("Server=./SQLEXPRESS;Database=SmartProp2;Integrated Security=SSPI;server=(local)");

                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<User>()
          .HasIndex(p => new { p.Email, p.PersonalID })
          .IsUnique(true);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

