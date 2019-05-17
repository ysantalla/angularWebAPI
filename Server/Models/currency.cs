using Server.Models;
using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class Currency : BaseModel
    {
        public long CurrencyId { get; set; }
        public string Name { get; set; }
        public string Simbol { get; set; }
    }
}