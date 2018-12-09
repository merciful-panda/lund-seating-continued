using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LundSeating.Models;

namespace LundSeating.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<EventSponsor> EventSponsors { get; set; }
        public DbSet<EventSeat> EventSeats { get; set; }
        public DbSet<EventGuest> EventGuests { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Event>()
                .HasKey(e => e.EventId);
            builder.Entity<Event>()
                .HasAlternateKey(e => new { e.Name, e.Date });

            builder.Entity<Sponsor>()
                .HasKey(s => s.SponsorID);
            builder.Entity<Sponsor>()
                .HasAlternateKey(s => new { s.Name });

            builder.Entity<Seat>()
                .HasKey(s => s.SeatID);
            builder.Entity<Seat>()
                .HasAlternateKey(s => new { s.Section, s.Row, s.Number });

            builder.Entity<Guest>()
                .HasKey(g => g.GuestID);
            

            builder.Entity<EventSponsor>()
                .HasKey(es => es.EventSponsorID);
            builder.Entity<EventSponsor>()
                .HasOne(es => es.Event)
                .WithMany(e => e.Sponsors)
                .HasForeignKey(es => es.EventID);
            builder.Entity<EventSponsor>()
                .HasOne(es => es.Sponsor)
                .WithMany(s => s.Events)
                .HasForeignKey(es => es.SponsorID);
            builder.Entity<EventSponsor>()
                .HasOne(es => es.Host)
                .WithMany(g => g.HostedEvents)
                .HasForeignKey(es => es.HostID);

            builder.Entity<EventSeat>()
                .HasKey(es => es.EventSeatID);
            builder.Entity<EventSeat>()
                .HasOne(es => es.Event)
                .WithMany(e => e.Seats)
                .HasForeignKey(es => es.EventID);
            builder.Entity<EventSeat>()
                .HasOne(es => es.Seat)
                .WithMany(e => e.Events)
                .HasForeignKey(es => es.SeatID);
            builder.Entity<EventSeat>()
                .HasOne(es => es.Sponsor)
                .WithMany(e => e.Seats)
                .HasForeignKey(es => es.EventSponsorID);
            builder.Entity<EventSeat>()
                .HasOne(es => es.Occupant);

            builder.Entity<EventGuest>()
                .HasKey(eg => eg.EventGuestID);
            builder.Entity<EventGuest>()
                .HasOne(eg => eg.Event)
                .WithMany(e => e.Guests)
                .HasForeignKey(eg => eg.EventID);
            builder.Entity<EventGuest>()
                .HasOne(eg => eg.Guest)
                .WithMany(e => e.Events)
                .HasForeignKey(eg => eg.GuestID);
            builder.Entity<EventGuest>()
                .HasOne(eg => eg.Sponsor)
                .WithMany(e => e.Guests)
                .HasForeignKey(eg => eg.EventSponsorID);
        }
    }
}
