using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LundSeating.Models
{
    public class Sponsor
    {
        public int SponsorID { get; set; }
        public String Name { get; set; }
        public String PhoneNumber { get; set; }
        public String EmailAddress { get; set; }

        public virtual List<EventSponsor> Events { get; } = new List<EventSponsor>();
    }
}
