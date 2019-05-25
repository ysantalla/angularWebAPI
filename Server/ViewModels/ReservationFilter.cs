using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class ReservationFilter
    {
        public string searchString { get; set; }
        
        public long guestID { get; set; }

        public long agencyID { get; set; }

        public long roomID { get; set; }

        public long packageID { get; set; }
    }
}