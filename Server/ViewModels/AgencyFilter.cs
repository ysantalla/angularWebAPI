using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class AgencyFilter
    {
        public string searchString { get; set; }
        
        public long countryID { get; set; }
    }
}