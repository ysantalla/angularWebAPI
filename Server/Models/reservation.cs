using Server.Models;
using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class Reservation : BaseModel
    {
        public long GuestID { get; set; }
        public Guest Guest { get; set; }
        public string Details { get; set; }
        public DateTime InitDate { get; set; }
        public DateTime EndDate { get; set; }

        public long AgencyID { get; set; }
        public Agency Agency {get; set; }  
        public long RoomID { get; set; }
        public Room Room {get; set;}
        public long PackageID { get; set; }
    }
}