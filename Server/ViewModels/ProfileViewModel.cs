using System;
using System.Collections.Generic;

namespace Server.ViewModels
{
    public class ProfileViewModel
    {
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }        
        public List<string> Roles {get; set;}

    }
}