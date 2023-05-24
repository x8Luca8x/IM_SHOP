﻿// <auto-generated />
using System;
using IM_API;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IM_API.Migrations
{
    [DbContext(typeof(IMDbContext))]
    [Migration("20230524183314_InitialShopMigration")]
    partial class InitialShopMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("IM_API.Models.TARTICLE", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CATEGORYID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CHANGED")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("CREATED")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CURRENCYID")
                        .HasColumnType("int");

                    b.Property<string>("DESCRIPTION")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("PERSONID")
                        .HasColumnType("int");

                    b.Property<string>("PRICE")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("SUBCATEGORYID")
                        .HasColumnType("int");

                    b.Property<string>("TITLE")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("VIEWS")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Article");
                });

            modelBuilder.Entity("IM_API.Models.TCATEGORY", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("DESCRIPTION")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NAME")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("PARENTID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("IM_API.Models.TCURRENCY", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("NAME")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("SYMBOL")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Currency");
                });

            modelBuilder.Entity("IM_API.Models.TTOKEN", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("CAN_EXPIRE")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("CHANGED")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("CREATED")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DEVICE_APP")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("DEVICE_NAME")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("DEVICE_OS")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("EXPIRES")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("TOKEN")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("USERID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("TOKEN")
                        .IsUnique();

                    b.ToTable("Token");
                });

            modelBuilder.Entity("IM_API.Models.TUSEROPTIONS", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CHANGED")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("CREATED")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("SHOW_BIRTHDATE")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("SHOW_CHANGED")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("SHOW_CREATED")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("SHOW_EMAIL")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("SHOW_FIRSTNAME")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("SHOW_LASTNAME")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("SHOW_ROLE")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("USERID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("USERID")
                        .IsUnique();

                    b.ToTable("UserOptions");
                });

            modelBuilder.Entity("IMAPI.Models.TIMAGE", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<byte[]>("DATA")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<int>("ENTITYID")
                        .HasColumnType("int");

                    b.Property<string>("ENTITYTYPE")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NAME")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("TYPE")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("IMAPI.Models.TPERSON", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CHANGED")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("CREATED")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DISPLAYNAME")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("USERID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("USERID")
                        .IsUnique();

                    b.ToTable("Person");
                });

            modelBuilder.Entity("IMAPI.Models.TUSER", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("ACTIVE")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("BIRTHDATE")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("CHANGED")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("CREATED")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("EMAIL")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("FIRSTNAME")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LASTNAME")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PASSWORD")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ROLE")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("USERNAME")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("VERIFIED")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("ID");

                    b.HasIndex("EMAIL")
                        .IsUnique();

                    b.HasIndex("USERNAME")
                        .IsUnique();

                    b.ToTable("User");
                });
#pragma warning restore 612, 618
        }
    }
}
