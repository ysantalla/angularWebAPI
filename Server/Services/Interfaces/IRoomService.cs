using System;
using Server.Models;
using Server.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services.Interfaces
{
    public interface IRoomService : ICrudService<Room, RoomFilter>
    {
        Task<ProcessResult<List<FreeRoom>>> ListFreeRoomsAsync(DateTime initialDate);
    }
}