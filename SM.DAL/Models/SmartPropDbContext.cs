using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
 

#nullable disable

namespace SM.DAL.Models
{
    public partial class SmartPropDbContext : DbContext
    {
        public SmartPropDbContext()
        {
        }

        public SmartPropDbContext(DbContextOptions<SmartPropDbContext> options)
            : base(options)
        {
        }
        public DbSet<ImagesRealEstate> ImagesRealEstate { get; set; }
        public DbSet<RealEstate> RealEstates { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
