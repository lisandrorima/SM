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
        public DbSet<RentContract> RentContracts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<CuponDePago> CuponDePagos { get; set; }

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
          .HasIndex(p => new { p.Email})
          .IsUnique(true);

            modelBuilder.Entity<User>()
      .HasIndex(p => new { p.PersonalID })
      .IsUnique(true);

            modelBuilder.Entity<Provincia>()
      .HasIndex(p => new { p.Nombre })
      .IsUnique(true);

            modelBuilder.Entity<RentContract>()
  .HasIndex(p => new { p.Hash})
  .IsUnique(true);

            modelBuilder.Entity<CuponDePago>()
  .HasIndex(p => new { p.HashCuponPago })
  .IsUnique(true);

            modelBuilder.Entity<CuponDePago>()
.HasAlternateKey(x => x.HashCuponPago);


            modelBuilder.Entity<RealEstate>()
             .HasMany(e => e.images)
    .WithOne(e => e.RealEstate)
    .OnDelete(DeleteBehavior.ClientCascade);


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

