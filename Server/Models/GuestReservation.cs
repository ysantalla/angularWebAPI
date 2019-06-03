using Server.Models;
using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class GuestReservation
    {
        public long GuestId { get; set; }

        public virtual Guest Guest { get; set; }

        public long ReservationId { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}