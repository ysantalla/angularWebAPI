using Server.Models;
using Server.Models.Enums;
using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class Invoice : BaseModel
    {
        
        public double Number { get; set; }
        public long CurrencyID {get; set;}
        public Currency Currency { get; set; }
        public State State {get; set;}
        public DateTime Date { get; set; }
        public long ReservationID {get; set;}
        public Reservation Reservation { get; set; }

    }
}