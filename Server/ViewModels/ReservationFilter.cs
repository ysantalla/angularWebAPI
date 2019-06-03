using System;
using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class ReservationFilter
    {
        public ReservationFilter() {
            this.agencyID = 0;
            this.roomID = 0;
            this.searchString = null;
            this.checkInDate = DateTime.MinValue;
            this.checkOutDate = DateTime.MinValue;
            this.checkInState = 0;
            this.checkOutState = 0;
        }

        public string searchString { get; set; }

        public long agencyID { get; set; }

        public long roomID { get; set; }

        public DateTime checkInDate { get; set; }

        public DateTime checkOutDate { get; set; }
    
        public int checkInState { get; set; }

        public int checkOutState { get; set; }
    }
}