using Server.Models;
using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class Invoice : BaseModel
    {
        
        public double Number { get; set; }

        public long CurrencyID {get; set;}
        public Currency Currency { get; set; }

        public DateTime Date { get; set; }

        public long GuestID {get; set;}
        public Guest Guest { get; set; }

    }
}