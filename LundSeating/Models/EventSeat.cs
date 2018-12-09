using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LundSeating.Models
{
    public class EventSeat
    {
        public int EventSeatID { get; set; }
        public int EventID { get; set; }
        public int SeatID { get; set; }
        public int? EventSponsorID { get; set; }
        public int? EventGuestID { get; set; }

        public virtual Event Event { get; set; }
        public virtual Seat Seat { get; set; }
        public virtual EventSponsor Sponsor { get; set; }
        public virtual EventGuest Occupant { get; set; }
    }
}
