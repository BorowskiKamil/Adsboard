﻿// <auto-generated />
using System;
using Adsboard.Services.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Adsboard.Services.Identity.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Adsboard.Services.Identity.Domain.Identity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email")
                        .HasColumnType("VARCHAR(255)");

                    b.Property<string>("PasswordHash")
                        .HasColumnName("password_hash")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Role")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("role")
                        .HasColumnType("varchar(255)")
                        .HasDefaultValue("user");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("identities");
                });

            modelBuilder.Entity("Adsboard.Services.Identity.Domain.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at");

                    b.Property<Guid>("IdentityId")
                        .HasColumnName("identity_id");

                    b.Property<DateTime?>("RevokedAt")
                        .HasColumnName("revoked_at");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnName("token")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("refresh_tokens");
                });
#pragma warning restore 612, 618
        }
    }
}