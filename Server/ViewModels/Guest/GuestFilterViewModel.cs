using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class GuestFilterViewModel
    {
        public string sortOrder { get; set; }
        public string searchString { get; set; }

        public long countryID { get; set; }
        public long citizenshipID { get; set; }

        public int pageIndex { get; set; }
        public int pageSize { get; set; }

    }
}