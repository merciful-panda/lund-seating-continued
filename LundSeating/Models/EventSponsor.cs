using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LundSeating.Models
{
    public enum SponsorshipLevel
    {
        Bronze,
        Silver,
        Gold,
        Platinum,
        SuperSpecial
    }

    public class EventSponsor
    {
        public int EventSponsorID { get; set; }
        public int EventID { get; set; }
        public int SponsorID { get; set; }
        public int? HostID { get; set; }
        public SponsorshipLevel? SponsorshipLevel { get; set; }
        public int? NumTicketsAllocated { get; set; }

        public virtual Event Event { get; set; }
        public virtual Sponsor Sponsor { get; set; }
        public virtual Guest Host { get; set; }

        public virtual List<EventSeat> Seats { get; } = new List<EventSeat>();
        public virtual List<EventGuest> Guests { get; } = new List<EventGuest>();
    }
}
