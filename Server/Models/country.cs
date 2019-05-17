using Server.Models;
using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class Country : BaseModel
    {
        public long CuntryId { get; set; }
        public string Name { get; set; }
    }
}