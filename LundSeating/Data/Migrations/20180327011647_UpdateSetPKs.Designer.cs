﻿// <auto-generated />
using LundSeating.Data;
using LundSeating.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace LundSeating.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180327011647_UpdateSetPKs")]
    partial class UpdateSetPKs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LundSeating.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("LundSeating.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<bool>("IsLocked");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("EventId");

                    b.HasAlternateKey("Name", "Date");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("LundSeating.Models.EventGuest", b =>
                {
                    b.Property<int>("EventGuestID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EventID");

                    b.Property<int?>("EventSeatID");

                    b.Property<int>("EventSponsorID");

                    b.Property<int>("GuestID");

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<int?>("Status");

                    b.HasKey("EventGuestID");

                    b.HasIndex("EventID");

                    b.HasIndex("EventSponsorID");

                    b.HasIndex("GuestID");

                    b.ToTable("EventGuests");
                });

            modelBuilder.Entity("LundSeating.Models.EventSeat", b =>
                {
                    b.Property<int>("EventSeatID");

                    b.Property<int?>("EventGuestID");

                    b.Property<int>("EventID");

                    b.Property<int?>("EventSponsorID");

                    b.Property<int>("SeatID");

                    b.HasKey("EventSeatID");

                    b.HasIndex("EventID");

                    b.HasIndex("EventSponsorID");

                    b.HasIndex("SeatID");

                    b.ToTable("EventSeats");
                });

            modelBuilder.Entity("LundSeating.Models.EventSponsor", b =>
                {
                    b.Property<int>("EventSponsorID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EventID");

                    b.Property<int?>("HostID");

                    b.Property<int?>("NumTicketsAllocated");

                    b.Property<int>("SponsorID");

                    b.Property<int?>("SponsorshipLevel");

                    b.HasKey("EventSponsorID");

                    b.HasIndex("EventID");

                    b.HasIndex("HostID");

                    b.HasIndex("SponsorID");

                    b.ToTable("EventSponsors");
                });

            modelBuilder.Entity("LundSeating.Models.Guest", b =>
                {
                    b.Property<int>("GuestID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("Name");

                    b.Property<string>("PhoneNumber");

                    b.HasKey("GuestID");

                    b.HasAlternateKey("EmailAddress");

                    b.ToTable("Guests");
                });

            modelBuilder.Entity("LundSeating.Models.Seat", b =>
                {
                    b.Property<int>("SeatID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Number")
                        .IsRequired();

                    b.Property<string>("Row")
                        .IsRequired();

                    b.Property<string>("Section")
                        .IsRequired();

                    b.HasKey("SeatID");

                    b.HasAlternateKey("Section", "Row", "Number");

                    b.ToTable("Seats");
                });

            modelBuilder.Entity("LundSeating.Models.Sponsor", b =>
                {
                    b.Property<int>("SponsorID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EmailAddress");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PhoneNumber");

                    b.HasKey("SponsorID");

                    b.HasAlternateKey("Name");

                    b.ToTable("Sponsors");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("LundSeating.Models.EventGuest", b =>
                {
                    b.HasOne("LundSeating.Models.Event", "Event")
                        .WithMany("Guests")
                        .HasForeignKey("EventID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LundSeating.Models.EventSponsor", "Sponsor")
                        .WithMany("Guests")
                        .HasForeignKey("EventSponsorID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LundSeating.Models.Guest", "Guest")
                        .WithMany("Events")
                        .HasForeignKey("GuestID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LundSeating.Models.EventSeat", b =>
                {
                    b.HasOne("LundSeating.Models.Event", "Event")
                        .WithMany("Seats")
                        .HasForeignKey("EventID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LundSeating.Models.EventGuest", "Occupant")
                        .WithOne("Seat")
                        .HasForeignKey("LundSeating.Models.EventSeat", "EventSeatID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LundSeating.Models.EventSponsor", "Sponsor")
                        .WithMany("Seats")
                        .HasForeignKey("EventSponsorID");

                    b.HasOne("LundSeating.Models.Seat", "Seat")
                        .WithMany("Events")
                        .HasForeignKey("SeatID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LundSeating.Models.EventSponsor", b =>
                {
                    b.HasOne("LundSeating.Models.Event", "Event")
                        .WithMany("Sponsors")
                        .HasForeignKey("EventID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LundSeating.Models.Guest", "Host")
                        .WithMany("HostedEvents")
                        .HasForeignKey("HostID");

                    b.HasOne("LundSeating.Models.Sponsor", "Sponsor")
                        .WithMany("Events")
                        .HasForeignKey("SponsorID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("LundSeating.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("LundSeating.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LundSeating.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("LundSeating.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
