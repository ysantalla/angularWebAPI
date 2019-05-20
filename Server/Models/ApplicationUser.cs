using Server.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
                
    }
}