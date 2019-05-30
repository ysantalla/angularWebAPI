using Server.Models;
using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class Room : BaseModel
    {
        
        public string Number { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public bool Enable { get; set; }

        public int BedCont { get; set; }

        public double VPN { get; set; }

    }
}