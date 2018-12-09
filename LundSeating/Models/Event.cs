using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LundSeating.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public String Name { get; set; }
        public DateTime Date { get; set; }
        public Boolean IsLocked { get; set; }

        public virtual List<EventSponsor> Sponsors { get; } = new List<EventSponsor>();
        public virtual List<EventSeat> Seats { get; } = new List<EventSeat>();
        public virtual List<EventGuest> Guests { get; } = new List<EventGuest>();
    }
}
