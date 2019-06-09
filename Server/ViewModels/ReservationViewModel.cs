using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Server.Models;

namespace Server.ViewModels
{
    public class ReservationViewModel
    {
        public long agencyID { get; set; }

        public long roomID { get; set; }

        public string Details { get; set; }
        public DateTime InitDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool CheckIn { get; set; }
        public bool CheckOut { get; set; }

        public IList<Guest> guests { get; set; }
    }
}