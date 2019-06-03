using Server.Models;
using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class Reservation : BaseModel
    {
        public string Details { get; set; }
        public DateTime InitDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool CheckIn { get; set; }
        public bool CheckOut { get; set; }

        public long AgencyID { get; set; }
        public Agency Agency {get; set; }  
        public long RoomID { get; set; }
        public Room Room { get; set;}
        public virtual IList<GuestReservation> GuestReservations { get; set; }
    }
}