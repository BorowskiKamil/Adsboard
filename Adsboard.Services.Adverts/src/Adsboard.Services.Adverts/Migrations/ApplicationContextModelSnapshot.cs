﻿// <auto-generated />
using System;
using Adsboard.Services.Adverts.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Adsboard.Services.Adverts.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Adsboard.Services.Adverts.Domain.Advert", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<Guid>("Category")
                        .HasColumnName("category_id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnName("created_at");

                    b.Property<Guid>("Creator")
                        .HasColumnName("creator_id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .HasColumnName("image")
                        .HasColumnType("VARCHAR(255)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title")
                        .HasColumnType("VARCHAR(255)");

                    b.HasKey("Id");

                    b.ToTable("adverts");
                });
#pragma warning restore 612, 618
        }
    }
}
