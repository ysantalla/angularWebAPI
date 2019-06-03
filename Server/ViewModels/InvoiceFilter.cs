using System;
using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class InvoiceFilter
    {
        public long guestID {get; set;}

        public long currencyID { get; set; }
    }
}