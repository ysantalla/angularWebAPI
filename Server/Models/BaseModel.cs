using System;

namespace Server.Models
{
    public abstract class BaseModel
    {
        public long Id { get; set; }

        public long CreatorId { get; set; }

        public long ModifierId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        // HVersion gets +1 whenever a record is updated
        public int HVersion { get; set; }
    }
}