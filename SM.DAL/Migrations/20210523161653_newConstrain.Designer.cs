﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SM.DAL.Models;

namespace SM.DAL.Migrations
{
    [DbContext(typeof(SmartPropDbContext))]
    [Migration("20210523161653_newConstrain")]
    partial class newConstrain
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("Localidad")
                        .HasColumnType("nvarchar(max)");

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

            modelBuilder.Entity("SM.DAL.Models.RentContract", b =>
                {
                    b.Property<int?>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("OwnerID")
                        .HasColumnType("int");

                    b.Property<int?>("RealEstateID")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("TenantID")
                        .HasColumnType("int");

                    b.Property<bool>("ValidatedByBlockChain")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.HasIndex("OwnerID");

                    b.HasIndex("RealEstateID");

                    b.HasIndex("TenantID");

                    b.ToTable("RentContracts");
                });

            modelBuilder.Entity("SM.DAL.Models.User", b =>
                {
                    b.Property<int?>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

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

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("PersonalID")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SM.DAL.Models.ImagesRealEstate", b =>
                {
                    b.HasOne("SM.DAL.Models.RealEstate", "RealEstate")
                        .WithMany("images")
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

            modelBuilder.Entity("SM.DAL.Models.RentContract", b =>
                {
                    b.HasOne("SM.DAL.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerID");

                    b.HasOne("SM.DAL.Models.RealEstate", "RealEstate")
                        .WithMany()
                        .HasForeignKey("RealEstateID");

                    b.HasOne("SM.DAL.Models.User", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantID");

                    b.Navigation("Owner");

                    b.Navigation("RealEstate");

                    b.Navigation("Tenant");
                });

            modelBuilder.Entity("SM.DAL.Models.RealEstate", b =>
                {
                    b.Navigation("images");
                });
#pragma warning restore 612, 618
        }
    }
}
