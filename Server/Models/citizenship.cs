using Server.Models;
using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class Citizenship : BaseModel
    {
        public long CitizenshipId { get; set; }
        public string Name { get; set; }
    }
}