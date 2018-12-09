using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LundSeating.Models
{
    public enum Status
    {
        Present,
        NoShow
    }

    public class EventGuest
    {
        public int EventGuestID { get; set; }
        public int EventID { get; set; }
        public int GuestID { get; set; }
        public int EventSponsorID { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Status? Status { get; set; }


        public virtual Event Event { get; set; }
        public virtual Guest Guest { get; set; }
        public virtual EventSponsor Sponsor { get; set; }
    }
}
