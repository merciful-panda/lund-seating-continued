using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LundSeating.Models;

namespace LundSeating.Data
{
    public class DbInitalizer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            Event @event;
            if (!context.Events.Any())
            {
                @event = new Event { Name = "Summer Concert 2017", Date = new DateTime(2017, 7, 1) };
                context.Add(@event);
                context.SaveChanges();
            }
            else
            {
                @event = context.Events.First();
            }

            List<Seat> @seats;
            if (!context.Seats.Any())
            {
                @seats = new List<Seat>();

                // Middle Section
                int[] @seatCounts = new int[] { 10, 11, 10, 11, 11, 12, 11, 12, 12, 13, 12, 13, 12, 13, 14, 13, 14 };
                string[] @rows = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q" };
                for (int i = 0; i < @rows.Length; i++)
                {
                    for (int j = 1; j <= @seatCounts[i]; j++)
                    {
                        Seat @seat = new Seat { Section = "Middle", Row = @rows[i], Number = j.ToString() };
                        context.Add(@seat);
                        @seats.Add(@seat);
                    }
                }

                // Side Sections
                int[] @seatCountsSides = new int[] { 5, 6, 8, 9, 10, 10, 11, 11, 12, 12, 12, 12, 12, 12, 12, 12 };
                string[] @rowsSides = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P" };
                string[] @sides = new string[] { "Left", "Right" };
                foreach (string @side in @sides)
                {
                    for (int i = 0; i < @rowsSides.Length; i++)
                    {
                        for (int j = 1; j <= @seatCountsSides[i]; j++)
                        {
                            Seat @seat = new Seat { Section = @side, Row = @rowsSides[i], Number = j.ToString() };
                            context.Add(@seat);
                            @seats.Add(@seat);
                        }
                    }
                }

                context.SaveChanges();
            }
            else
            {
                @seats = context.Seats.ToList();
            }

            if (!context.EventSeats.Where(es => es.EventID == @event.EventId).Any())
            {
                foreach (Seat seat in @seats)
                {
                    EventSeat es = new EventSeat { EventID = @event.EventId, SeatID = seat.SeatID };
                    context.Add(es);
                }

                context.SaveChanges();
            }
           

            if (!context.Sponsors.Any())
            {
                context.Sponsors.Add(new Sponsor { Name = "UNSPONSORED" });
                context.SaveChanges();
            }

        }
    }
}
