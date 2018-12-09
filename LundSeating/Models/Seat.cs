using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LundSeating.Models
{
    public class Seat
    {
        public int SeatID { get; set; }
        public String Section { get; set; }
        public String Row { get; set; }
        public String Number { get; set; }

        public virtual List<EventSeat> Events { get; } = new List<EventSeat>();
    }
}
