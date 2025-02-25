﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Orbitron.Data;

#nullable disable

namespace Orbitron.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250129102523_Initial Migration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Orbitron.Models.Call", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Ended")
                        .HasColumnType("datetime2");

                    b.Property<string>("PhoneReceiver")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneSender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Started")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Calls");
                });

            modelBuilder.Entity("Orbitron.Models.Package", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MB")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SMS")
                        .HasColumnType("int");

                    b.Property<int>("TotalMinutes")
                        .HasColumnType("int");

                    b.Property<int>("TotalMinutesAbroad")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Packages");
                });

            modelBuilder.Entity("Orbitron.Models.Phone", b =>
                {
                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Package")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("issuedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("renewDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Number");

                    b.ToTable("Phones");
                });

            modelBuilder.Entity("Orbitron.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Property")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Orbitron.Models.Administrator", b =>
                {
                    b.HasBaseType("Orbitron.Models.User");

                    b.ToTable("Administrators", (string)null);
                });

            modelBuilder.Entity("Orbitron.Models.Client", b =>
                {
                    b.HasBaseType("Orbitron.Models.User");

                    b.Property<string>("AFM")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Clients", (string)null);
                });

            modelBuilder.Entity("Orbitron.Models.Seller", b =>
                {
                    b.HasBaseType("Orbitron.Models.User");

                    b.ToTable("Sellers", (string)null);
                });

            modelBuilder.Entity("Orbitron.Models.Administrator", b =>
                {
                    b.HasOne("Orbitron.Models.User", null)
                        .WithOne()
                        .HasForeignKey("Orbitron.Models.Administrator", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Orbitron.Models.Client", b =>
                {
                    b.HasOne("Orbitron.Models.User", null)
                        .WithOne()
                        .HasForeignKey("Orbitron.Models.Client", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Orbitron.Models.Seller", b =>
                {
                    b.HasOne("Orbitron.Models.User", null)
                        .WithOne()
                        .HasForeignKey("Orbitron.Models.Seller", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
