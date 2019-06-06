using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class GuestFilter
    {
        public string searchString { get; set; }

        public string identification { get; set; }

        public long countryID { get; set; }

        public long citizenshipID { get; set; }
    }
}