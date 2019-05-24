using Server.Models;
using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class Package : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }

    }
}