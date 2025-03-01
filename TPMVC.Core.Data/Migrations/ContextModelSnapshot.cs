﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TPMVC.Core.Data;

#nullable disable

namespace TPMVC.Core.Data.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasDiscriminator().HasValue("IdentityUser");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.Brand", b =>
                {
                    b.Property<int>("BrandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BrandId"));

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("BrandId");

                    b.HasIndex("BrandName")
                        .IsUnique();

                    b.ToTable("Brands");

                    b.HasData(
                        new
                        {
                            BrandId = 1,
                            Active = true,
                            BrandName = "Vans"
                        },
                        new
                        {
                            BrandId = 2,
                            Active = true,
                            BrandName = "Adidas"
                        },
                        new
                        {
                            BrandId = 3,
                            Active = true,
                            BrandName = "Topper"
                        });
                });

            modelBuilder.Entity("TPMVC.Core.Entities.City", b =>
                {
                    b.Property<int>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CityId"));

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<int>("StateId")
                        .HasColumnType("int");

                    b.HasKey("CityId");

                    b.HasIndex("CityName")
                        .IsUnique();

                    b.HasIndex("CountryId");

                    b.HasIndex("StateId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.Colour", b =>
                {
                    b.Property<int>("ColourId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ColourId"));

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("ColourName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ColourId");

                    b.HasIndex("ColourName")
                        .IsUnique();

                    b.ToTable("Colours");

                    b.HasData(
                        new
                        {
                            ColourId = 1,
                            Active = true,
                            ColourName = "Rojo"
                        },
                        new
                        {
                            ColourId = 2,
                            Active = true,
                            ColourName = "Negro"
                        },
                        new
                        {
                            ColourId = 3,
                            Active = true,
                            ColourName = "Blanco"
                        });
                });

            modelBuilder.Entity("TPMVC.Core.Entities.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CountryId"));

                    b.Property<string>("CountryName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CountryId");

                    b.HasIndex("CountryName")
                        .IsUnique();

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.Genre", b =>
                {
                    b.Property<int>("GenreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GenreId"));

                    b.Property<string>("GenreName")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("GenreId");

                    b.HasIndex("GenreName")
                        .IsUnique();

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            GenreId = 1,
                            GenreName = "Femenino"
                        },
                        new
                        {
                            GenreId = 2,
                            GenreName = "Masculino"
                        },
                        new
                        {
                            GenreId = 3,
                            GenreName = "Unisex"
                        });
                });

            modelBuilder.Entity("TPMVC.Core.Entities.Shoe", b =>
                {
                    b.Property<int>("ShoeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShoeId"));

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<int>("ColourId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<decimal>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("SportId")
                        .HasColumnType("int");

                    b.Property<string>("imageURL")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ShoeId");

                    b.HasIndex("BrandId");

                    b.HasIndex("ColourId");

                    b.HasIndex("GenreId");

                    b.HasIndex("SportId");

                    b.ToTable("Shoes");

                    b.HasData(
                        new
                        {
                            ShoeId = 1,
                            Active = true,
                            BrandId = 1,
                            ColourId = 1,
                            Description = "Vans Deportivas",
                            GenreId = 2,
                            Model = "Deportivas",
                            Price = 15m,
                            SportId = 3
                        },
                        new
                        {
                            ShoeId = 2,
                            Active = true,
                            BrandId = 2,
                            ColourId = 2,
                            Description = "Botines Femeninos",
                            GenreId = 1,
                            Model = "Botines",
                            Price = 20m,
                            SportId = 1
                        },
                        new
                        {
                            ShoeId = 3,
                            Active = true,
                            BrandId = 3,
                            ColourId = 1,
                            Description = "Importados",
                            GenreId = 3,
                            Model = "1982",
                            Price = 35m,
                            SportId = 2
                        });
                });

            modelBuilder.Entity("TPMVC.Core.Entities.ShoeSize", b =>
                {
                    b.Property<int?>("ShoeSizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("ShoeSizeId"));

                    b.Property<int>("QuantityInStock")
                        .HasColumnType("int");

                    b.Property<int>("ShoeId")
                        .HasColumnType("int");

                    b.Property<int>("SizeId")
                        .HasColumnType("int");

                    b.HasKey("ShoeSizeId");

                    b.HasIndex("SizeId");

                    b.HasIndex("ShoeId", "SizeId")
                        .IsUnique();

                    b.ToTable("ShoesSizes", (string)null);
                });

            modelBuilder.Entity("TPMVC.Core.Entities.ShoppingCart", b =>
                {
                    b.Property<int>("ShoppingCartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShoppingCartId"));

                    b.Property<string>("ApplicationUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("ShoeSizeId")
                        .HasColumnType("int");

                    b.HasKey("ShoppingCartId");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("ShoeSizeId");

                    b.HasIndex("ShoppingCartId")
                        .IsUnique();

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.Size", b =>
                {
                    b.Property<int>("SizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SizeId"));

                    b.Property<decimal>("SizeNumber")
                        .HasPrecision(3, 1)
                        .HasColumnType("decimal (3, 1)");

                    b.HasKey("SizeId");

                    b.HasIndex("SizeNumber")
                        .IsUnique();

                    b.ToTable("Sizes", (string)null);

                    b.HasData(
                        new
                        {
                            SizeId = 1,
                            SizeNumber = 28m
                        },
                        new
                        {
                            SizeId = 2,
                            SizeNumber = 28.5m
                        },
                        new
                        {
                            SizeId = 3,
                            SizeNumber = 29.0m
                        },
                        new
                        {
                            SizeId = 4,
                            SizeNumber = 29.5m
                        },
                        new
                        {
                            SizeId = 5,
                            SizeNumber = 30.0m
                        },
                        new
                        {
                            SizeId = 6,
                            SizeNumber = 30.5m
                        },
                        new
                        {
                            SizeId = 7,
                            SizeNumber = 31.0m
                        },
                        new
                        {
                            SizeId = 8,
                            SizeNumber = 31.5m
                        },
                        new
                        {
                            SizeId = 9,
                            SizeNumber = 32.0m
                        },
                        new
                        {
                            SizeId = 10,
                            SizeNumber = 32.5m
                        },
                        new
                        {
                            SizeId = 11,
                            SizeNumber = 33.0m
                        },
                        new
                        {
                            SizeId = 12,
                            SizeNumber = 33.5m
                        },
                        new
                        {
                            SizeId = 13,
                            SizeNumber = 34.0m
                        },
                        new
                        {
                            SizeId = 14,
                            SizeNumber = 34.5m
                        },
                        new
                        {
                            SizeId = 15,
                            SizeNumber = 35.0m
                        },
                        new
                        {
                            SizeId = 16,
                            SizeNumber = 35.5m
                        },
                        new
                        {
                            SizeId = 17,
                            SizeNumber = 36.0m
                        },
                        new
                        {
                            SizeId = 18,
                            SizeNumber = 36.5m
                        },
                        new
                        {
                            SizeId = 19,
                            SizeNumber = 37.0m
                        },
                        new
                        {
                            SizeId = 20,
                            SizeNumber = 37.5m
                        },
                        new
                        {
                            SizeId = 21,
                            SizeNumber = 38.0m
                        },
                        new
                        {
                            SizeId = 22,
                            SizeNumber = 38.5m
                        },
                        new
                        {
                            SizeId = 23,
                            SizeNumber = 39.0m
                        },
                        new
                        {
                            SizeId = 24,
                            SizeNumber = 39.5m
                        },
                        new
                        {
                            SizeId = 25,
                            SizeNumber = 40.0m
                        },
                        new
                        {
                            SizeId = 26,
                            SizeNumber = 40.5m
                        },
                        new
                        {
                            SizeId = 27,
                            SizeNumber = 41.0m
                        },
                        new
                        {
                            SizeId = 28,
                            SizeNumber = 41.5m
                        },
                        new
                        {
                            SizeId = 29,
                            SizeNumber = 42.0m
                        },
                        new
                        {
                            SizeId = 30,
                            SizeNumber = 42.5m
                        },
                        new
                        {
                            SizeId = 31,
                            SizeNumber = 43.0m
                        },
                        new
                        {
                            SizeId = 32,
                            SizeNumber = 43.5m
                        },
                        new
                        {
                            SizeId = 33,
                            SizeNumber = 44.0m
                        },
                        new
                        {
                            SizeId = 34,
                            SizeNumber = 44.5m
                        },
                        new
                        {
                            SizeId = 35,
                            SizeNumber = 45.0m
                        },
                        new
                        {
                            SizeId = 36,
                            SizeNumber = 45.5m
                        },
                        new
                        {
                            SizeId = 37,
                            SizeNumber = 46.0m
                        },
                        new
                        {
                            SizeId = 38,
                            SizeNumber = 46.5m
                        },
                        new
                        {
                            SizeId = 39,
                            SizeNumber = 47.0m
                        },
                        new
                        {
                            SizeId = 40,
                            SizeNumber = 47.5m
                        },
                        new
                        {
                            SizeId = 41,
                            SizeNumber = 48.0m
                        },
                        new
                        {
                            SizeId = 42,
                            SizeNumber = 48.5m
                        },
                        new
                        {
                            SizeId = 43,
                            SizeNumber = 49.0m
                        },
                        new
                        {
                            SizeId = 44,
                            SizeNumber = 49.5m
                        },
                        new
                        {
                            SizeId = 45,
                            SizeNumber = 50.0m
                        });
                });

            modelBuilder.Entity("TPMVC.Core.Entities.Sport", b =>
                {
                    b.Property<int>("SportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SportId"));

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("SportName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("SportId");

                    b.HasIndex("SportName")
                        .IsUnique();

                    b.ToTable("Sports");

                    b.HasData(
                        new
                        {
                            SportId = 1,
                            Active = true,
                            SportName = "Futbol"
                        },
                        new
                        {
                            SportId = 2,
                            Active = true,
                            SportName = "Tenis"
                        },
                        new
                        {
                            SportId = 3,
                            Active = true,
                            SportName = "Basquet"
                        });
                });

            modelBuilder.Entity("TPMVC.Core.Entities.State", b =>
                {
                    b.Property<int>("StateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StateId"));

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("StateName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("StateId");

                    b.HasIndex("CountryId");

                    b.HasIndex("StateName")
                        .IsUnique();

                    b.ToTable("States");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StateId")
                        .HasColumnType("int");

                    b.Property<string>("Zipcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("CityId");

                    b.HasIndex("CountryId");

                    b.HasIndex("StateId");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.City", b =>
                {
                    b.HasOne("TPMVC.Core.Entities.Country", "Country")
                        .WithMany("Cities")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TPMVC.Core.Entities.State", "States")
                        .WithMany("Cities")
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");

                    b.Navigation("States");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.Shoe", b =>
                {
                    b.HasOne("TPMVC.Core.Entities.Brand", "Brand")
                        .WithMany("Shoes")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TPMVC.Core.Entities.Colour", "Colour")
                        .WithMany("Shoes")
                        .HasForeignKey("ColourId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TPMVC.Core.Entities.Genre", "Genre")
                        .WithMany("Shoes")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TPMVC.Core.Entities.Sport", "Sport")
                        .WithMany("Shoes")
                        .HasForeignKey("SportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("Colour");

                    b.Navigation("Genre");

                    b.Navigation("Sport");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.ShoeSize", b =>
                {
                    b.HasOne("TPMVC.Core.Entities.Shoe", "Shoe")
                        .WithMany("ShoesSizes")
                        .HasForeignKey("ShoeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TPMVC.Core.Entities.Size", "Size")
                        .WithMany("ShoesSizes")
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shoe");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.ShoppingCart", b =>
                {
                    b.HasOne("TPMVC.Core.Entities.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TPMVC.Core.Entities.ShoeSize", "ShoeSize")
                        .WithMany()
                        .HasForeignKey("ShoeSizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("ShoeSize");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.State", b =>
                {
                    b.HasOne("TPMVC.Core.Entities.Country", "Country")
                        .WithMany("States")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.ApplicationUser", b =>
                {
                    b.HasOne("TPMVC.Core.Entities.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TPMVC.Core.Entities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TPMVC.Core.Entities.State", "State")
                        .WithMany()
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");

                    b.Navigation("Country");

                    b.Navigation("State");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.Brand", b =>
                {
                    b.Navigation("Shoes");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.Colour", b =>
                {
                    b.Navigation("Shoes");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.Country", b =>
                {
                    b.Navigation("Cities");

                    b.Navigation("States");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.Genre", b =>
                {
                    b.Navigation("Shoes");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.Shoe", b =>
                {
                    b.Navigation("ShoesSizes");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.Size", b =>
                {
                    b.Navigation("ShoesSizes");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.Sport", b =>
                {
                    b.Navigation("Shoes");
                });

            modelBuilder.Entity("TPMVC.Core.Entities.State", b =>
                {
                    b.Navigation("Cities");
                });
#pragma warning restore 612, 618
        }
    }
}
