using Server.Models;
using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class Currency : BaseModel
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}