using Server.Models;
using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class Agency : BaseModel
    {        
        public string Name { get; set; }
        public string Represent { get; set; }

        public long CountryID { get; set; }
        public Country Country { get; set; } 
        public string Email { get; set; }  
        public string Phone { get; set; }

    }
}