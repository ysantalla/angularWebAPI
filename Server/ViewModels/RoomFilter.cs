using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class RoomFilter
    {
        public string searchString { get; set; }
        
        public int Capacity { get; set; }

        public int BedCont { get; set; }
    }
}