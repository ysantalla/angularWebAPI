using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Server.Services.Interfaces;
using Server.Models;
using Server.ViewModels;

namespace Server.Services
{
    public class RoomService : BaseService, IRoomService
    {
        public RoomService(UserManager<ApplicationUser> userManager,
                              IHttpContextAccessor contextAccessor,
                              ApplicationDbContext context)
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult> CreateAsync(Room model)
        {
            Func<Task> action = async () =>
            {

                var RoomExist = await context.Rooms.Where(x => x.Number == model.Number).CountAsync();

                if (RoomExist > 0)
                {
                    throw new InvalidOperationException("Ya existe un cuarto con el mismo número");
                }

                var RoomEntity = await GetOrCreateEntityAsync(context.Rooms, x => x.Id == model.Id);
                var Room = RoomEntity.result;

                Room.Number = model.Number;
                Room.Description = model.Description;
                Room.Capacity = model.Capacity;
                Room.BedCont = model.BedCont;
                Room.VPN = model.VPN;

                await context.SaveChangesAsync();
            };
            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<Room>> RetrieveAsync(long id)
        {
            Func<Task<Room>> action = async () =>
            {
                var result = await context.Rooms.Where(x => x.Id == id).FirstAsync();
                return result;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> UpdateAsync(long id, Room model)
        {
            model.Id = id;

            Func<Task> action = async () =>
            {
                var RoomExist = await context.Rooms.Where(x => (x.Number == model.Number && x.Id != model.Id)).CountAsync();

                if (RoomExist > 0)
                {
                    throw new InvalidOperationException("Ya existe un cuarto con el mismo número");
                }

                var RoomEntity = await GetOrCreateEntityAsync(context.Rooms, x => x.Id == model.Id);
                var Room = RoomEntity.result;

                Room.Number = model.Number;
                Room.Description = model.Description;
                Room.Capacity = model.Capacity;
                Room.BedCont = model.BedCont;
                Room.VPN = model.VPN;

                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> DeleteAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var o = await context.Rooms.Where(x => x.Id == id).SingleAsync();
                context.Remove(o);
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }


        public async Task<ProcessResult<List<Room>>> ListAsync(GetListViewModel<RoomFilter> getListModel)
        {
            IQueryable<Room> q = context.Rooms;
            q = SetIncludes(q);
            q = SetFilter(q, getListModel.filter);

            var countItems = await q.CountAsync();

            q = SetPaginator(q, getListModel.paginator);
            q = SetOrderBy(q, getListModel.orderBy);

            Func<Task<List<Room>>> action = async () =>
            {
                var result = await q.ToListAsync();
                return result;
            };

            return await Process.RunAsync(action, countItems);
        }

        public async Task<ProcessResult<int>> CountAsync(RoomFilter filter)
        {
            IQueryable<Room> q = context.Rooms;
            q = SetFilter(q, filter);

            Func<Task<int>> action = async () =>
            {
                var countItems = await q.CountAsync();
                return countItems;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<List<FreeRoom>>> ListFreeRoomsAsync(DateTime initialDate) {
            initialDate = new DateTime(
                initialDate.Year, initialDate.Month, initialDate.Day, 9, 0, 0
            );
            
            
            IQueryable<Room> q = context.Rooms;
            List<Room> rooms = await q.OrderByDescending(o => o.Capacity).ToListAsync();

            List<FreeRoom> frooms = new List<FreeRoom>();

            for ( int i = 0; i < rooms.Count; i++ ) {
                Room room = rooms[i];

                var cnt = await context.Reservations.Where(
                    o => o.RoomID == room.Id && initialDate < o.EndDate
                ).CountAsync();

                if ( cnt == 0 ) {
                    frooms.Add( new FreeRoom(room, 1000000) );
                }
                else if ( cnt > 0 ) {
                    Reservation r = await context.Reservations.Where(
                        o => o.InitDate > initialDate || o.EndDate > initialDate
                    ).FirstAsync();
                
                    if ( r.InitDate > initialDate ) {
                        frooms.Add(new FreeRoom( room, r.InitDate.Subtract(initialDate).Days ));
                    }
                }
            }

            Func<Task<List<FreeRoom>>> action = async () =>
            {
                return frooms;
            };

            return await Process.RunAsync(action, frooms.Count);
        }

        private IQueryable<Room> SetIncludes(IQueryable<Room> q){
            return q;
        }

        private IQueryable<Room> SetOrderBy(IQueryable<Room> q, OrderBy ob) {
            if ( ob == null ) {
                return q;
            }

            if ( !ob.desc ) {
                if ( ob.by == "number" ) {
                    q = q.OrderBy(s => s.Number);
                }
                else if ( ob.by == "capacity" ) {
                    q = q.OrderBy(s => s.Capacity);
                } 
                else if ( ob.by == "bedCont" ) {
                    q = q.OrderBy(s => s.BedCont);
                }
                else if ( ob.by == "VPN" ) {
                    q = q.OrderBy(s => s.VPN);
                } 
                else {
                    q = q.OrderBy(s => s.Id);
                }
            }
            else {
                if ( ob.by == "number" ) {
                    q = q.OrderByDescending(s => s.Number);
                }
                else if ( ob.by == "capacity" ) {
                    q = q.OrderByDescending(s => s.Capacity);
                } 
                else if ( ob.by == "bedCont" ) {
                    q = q.OrderByDescending(s => s.BedCont);
                }
                else if ( ob.by == "VPN" ) {
                    q = q.OrderByDescending(s => s.VPN);
                } 
                else {
                    q = q.OrderByDescending(s => s.Id);
                }
            }
            return q;
        }

        private IQueryable<Room> SetFilter(IQueryable<Room> q, RoomFilter f) {
            if ( f == null ) {
                return q;
            }
            if (!String.IsNullOrEmpty(f.searchString))
            {
                q = q.Where(s => (
                    s.Number.Contains(f.searchString) ||
                    s.Description.Contains(f.searchString)
                ));
            }
            if (f.Capacity > 0) {
                q = q.Where( s => s.Capacity == f.Capacity );
            }
            if (f.BedCont > 0) {
                q = q.Where( s => s.BedCont == f.BedCont );
            }
            if (f.VPN > 0) {
                q = q.Where( s => s.VPN == f.VPN );
            }
            return q;
        }
    
        private IQueryable<Room> SetPaginator(IQueryable<Room> q, Paginator p) {
            if ( p == null ) {
                return q;
            }
            return q.Skip(p.offset).Take(p.limit);
        }
    }
}