using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LundSeating.Models
{
    public class Guest
    {
        public int GuestID { get; set; }
        public String Name { get; set; }
        public String Address { get; set; }
        public String PhoneNumber { get; set; }
        public String EmailAddress { get; set; }

        public virtual List<EventSponsor> HostedEvents { get; } = new List<EventSponsor>();
        public virtual List<EventGuest> Events { get; } = new List<EventGuest>();
    }
}
