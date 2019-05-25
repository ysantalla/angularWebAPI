using System;
using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class InvoiceFilter
    {
        public string searchString { get; set; }
        
        public long guestID {get; set;}

        public long currencyID { get; set; }
    }
}