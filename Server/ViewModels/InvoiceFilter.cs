using System;
using System.ComponentModel.DataAnnotations;
using Server.Models.Enums;

namespace Server.ViewModels
{
    public class InvoiceFilter
    {
        public long reservationID {get; set;}
        public long currencyID { get; set; }
        public DateTime Date { get; set; }
        public State State {get; set;}
    }
}