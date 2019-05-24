using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class RoleFilterViewModel
    {
        public string sortOrder { get; set; }
        public string searchString { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }

    }
}