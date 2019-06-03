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

        public int BedCont { get; set; }

        public double VPN { get; set; }

    }

    public class FreeRoom {
        public FreeRoom(Room room, int FreeDays) {
            this.Room = room;
            this.FreeDays = FreeDays;
        }

        public Room Room { get; set; }

        public int FreeDays { get; set; }
    }
}