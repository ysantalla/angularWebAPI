using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class AddRoleToUserViewModel
    {
        public long UserId { get; set; }
        public List<string> roles { get; set;}
    }
}