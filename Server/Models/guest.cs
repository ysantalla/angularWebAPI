using Server.Models;
using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class Guest : BaseModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Identification { get; set; }

        public virtual IList<GuestReservation> GuestReservations { get; set; }

        public long CountryID { get; set; }
        public Country Country { get; set; }

        public long CitizenshipID { get; set; }
        public Citizenship Citizenship { get; set; }
    }
}