﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SM.DAL.Models;

namespace SM.DAL.Migrations
{
    [DbContext(typeof(SmartPropDbContext))]
    partial class SmartPropDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SM.DAL.Models.ImagesRealEstate", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ImgURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RealEstateID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("RealEstateID");

                    b.ToTable("ImagesRealEstate");
                });

            modelBuilder.Entity("SM.DAL.Models.RealEstate", b =>
                {
                    b.Property<int?>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Available")
                        .HasColumnType("bit");

                    b.Property<int>("BathRoomQty")
                        .HasColumnType("int");

                    b.Property<int>("BedRoomQty")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Garage")
                        .HasColumnType("bit");

                    b.Property<int>("RentDurationDays")
                        .HasColumnType("int");

                    b.Property<int>("RentFee")
                        .HasColumnType("int");

                    b.Property<int>("RentPaymentSchedule")
                        .HasColumnType("int");

                    b.Property<int>("Rooms")
                        .HasColumnType("int");

                    b.Property<float>("SqMtrs")
                        .HasColumnType("real");

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("RealEstates");
                });

            modelBuilder.Entity("SM.DAL.Models.User", b =>
                {
                    b.Property<int?>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(100)");

                    b.Property<int>("PersonalID")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(100)");

                    b.Property<string>("WalletAddress")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(42)");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SM.DAL.Models.ImagesRealEstate", b =>
                {
                    b.HasOne("SM.DAL.Models.RealEstate", "RealEstate")
                        .WithMany()
                        .HasForeignKey("RealEstateID");

                    b.Navigation("RealEstate");
                });

            modelBuilder.Entity("SM.DAL.Models.RealEstate", b =>
                {
                    b.HasOne("SM.DAL.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
