using Server.Models;
using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class Invoice : BaseModel
    {
        
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public Guest Guest { get; set; }
        public Currency Currency { get; set; }

    }
}