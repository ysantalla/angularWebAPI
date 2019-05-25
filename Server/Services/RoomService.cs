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
                Room.Enable = model.Enable;
                Room.BedCont = model.BedCont;

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
                Room.Enable = model.Enable;
                Room.BedCont = model.BedCont;

                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> DeleteAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var Room = await context.Rooms.Where(x => x.Id == id).SingleAsync();

                Room.IsDeleted = true;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> RestoreAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var Room = await context.Rooms.Where(x => x.Id == id).SingleAsync();

                Room.IsDeleted = false;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<List<Room>>> ListAsync(GetListViewModel<RoomFilter> getListModel)
        {
            IQueryable<Room> q = context.Rooms;
            q = SetIncludes(q);
            q = q.Where(s => !s.IsDeleted);
            q = SetFilter(q, getListModel.filter);
            q = SetPaginator(q, getListModel.paginator);
            q = SetOrderBy(q, getListModel.orderBy);

            Func<Task<List<Room>>> action = async () =>
            {
                var result = await q.ToListAsync();
                return result;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<int>> CountAsync(RoomFilter filter)
        {
            IQueryable<Room> q = context.Rooms;
            q = q.Where(s => !s.IsDeleted);
            q = SetFilter(q, filter);

            Func<Task<int>> action = async () =>
            {
                var countItems = await q.CountAsync();
                return countItems;
            };

            return await Process.RunAsync(action);
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