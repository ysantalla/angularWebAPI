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
    public class ReservationService : BaseService, IReservationService
    {
        public ReservationService(UserManager<ApplicationUser> userManager,
                              IHttpContextAccessor contextAccessor,
                              ApplicationDbContext context)
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult> CreateAsync(Reservation model)
        {
            Func<Task> action = async () =>
            {

                var ReservationEntity = await GetOrCreateEntityAsync(context.Reservations, x => x.Id == model.Id);
                var Reservation = ReservationEntity.result;

                Reservation.GuestID = model.GuestID;
                Reservation.Details = model.Details;
                Reservation.InitDate = model.InitDate;
                Reservation.EndDate = model.EndDate;
                Reservation.AgencyID = model.AgencyID;
                Reservation.RoomID = model.RoomID;
                Reservation.PackageID = model.PackageID;
                
                await context.SaveChangesAsync();
            };
            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<Reservation>> RetrieveAsync(long id)
        {
            Func<Task<Reservation>> action = async () =>
            {
                var result = await context.Reservations.Where(x => x.Id == id).FirstAsync();
                return result;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> UpdateAsync(long id, Reservation model)
        {
            model.Id = id;

            Func<Task> action = async () =>
            {
                var ReservationEntity = await GetOrCreateEntityAsync(context.Reservations, x => x.Id == model.Id);
                var Reservation = ReservationEntity.result;

                Reservation.GuestID = model.GuestID;
                Reservation.Details = model.Details;
                Reservation.InitDate = model.InitDate;
                Reservation.EndDate = model.EndDate;
                Reservation.AgencyID = model.AgencyID;
                Reservation.RoomID = model.RoomID;
                Reservation.PackageID = model.PackageID;

                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> DeleteAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var o = await context.Reservations.Where(x => x.Id == id).SingleAsync();
                context.Remove(o);
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }


        public async Task<ProcessResult<List<Reservation>>> ListAsync(GetListViewModel<ReservationFilter> getListModel)
        {
            IQueryable<Reservation> q = context.Reservations;
            q = SetIncludes(q);
            q = SetFilter(q, getListModel.filter);

            var countItems = await q.CountAsync();

            q = SetPaginator(q, getListModel.paginator);
            q = SetOrderBy(q, getListModel.orderBy);

            Func<Task<List<Reservation>>> action = async () =>
            {
                var result = await q.ToListAsync();
                return result;
            };

            return await Process.RunAsync(action, countItems);
        }

        public async Task<ProcessResult<int>> CountAsync(ReservationFilter filter)
        {
            IQueryable<Reservation> q = context.Reservations;
            q = SetFilter(q, filter);

            Func<Task<int>> action = async () =>
            {
                var countItems = await q.CountAsync();
                return countItems;
            };

            return await Process.RunAsync(action);
        }

        private IQueryable<Reservation> SetIncludes(IQueryable<Reservation> q){
            q = q.Include( s => s.Guest );
            q = q.Include( s => s.Agency );
            q = q.Include( s => s.Room );
            return q;
        }

        private IQueryable<Reservation> SetOrderBy(IQueryable<Reservation> q, OrderBy ob) {
            if ( ob == null ) {
                return q;
            }

            if ( !ob.desc ) {
                if ( ob.by == "initDate" ) {
                    q = q.OrderBy(s => s.InitDate);
                } 
                else if ( ob.by == "endDate" ) {
                    q = q.OrderBy(s => s.EndDate);
                } 
                else {
                    q = q.OrderBy(s => s.Id);
                }
            }
            else {
                if ( ob.by == "initDate" ) {
                    q = q.OrderByDescending(s => s.InitDate);
                } 
                else if ( ob.by == "endDate" ) {
                    q = q.OrderByDescending(s => s.EndDate);
                } 
                else {
                    q = q.OrderByDescending(s => s.Id);
                }
            }
            return q;
        }

        private IQueryable<Reservation> SetFilter(IQueryable<Reservation> q, ReservationFilter f) {
            if ( f == null ) {
                return q;
            }
            if (!String.IsNullOrEmpty(f.searchString))
            {
                q = q.Where(s => 
                    s.Details.Contains(f.searchString)
                );
            }
            if (f.guestID > 0) {
                q = q.Where( s => s.GuestID == f.guestID );
            }
            if (f.agencyID > 0) {
                q = q.Where( s => s.AgencyID == f.agencyID );
            }
            if (f.roomID > 0) {
                q = q.Where( s => s.RoomID == f.roomID );
            }
            return q;
        }
    
        private IQueryable<Reservation> SetPaginator(IQueryable<Reservation> q, Paginator p) {
            if ( p == null ) {
                return q;
            }
            return q.Skip(p.offset).Take(p.limit);
        }
    }
}