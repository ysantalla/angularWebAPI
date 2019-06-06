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
                model.InitDate = new DateTime(model.InitDate.Year, model.InitDate.Month, model.InitDate.Day, 9, 0, 0);
                model.EndDate = new DateTime(model.EndDate.Year, model.EndDate.Month, model.EndDate.Day, 8, 59, 59);

                if ( model.EndDate < model.InitDate ) {
                    throw new InvalidOperationException("La fecha de terminación no puede ser antes de la fecha de inicio");
                }

                IQueryable<Reservation> q = context.Reservations;
                q = q.Where(r => r.RoomID == model.RoomID && !(r.EndDate.CompareTo(model.InitDate) < 0 || model.EndDate.CompareTo(r.InitDate) < 0));
                int cnt = await q.CountAsync();
                if (cnt > 0)
                {
                    throw new InvalidOperationException("Existe un conflicto con otra reservación");
                }

                var ReservationEntity = await GetOrCreateEntityAsync(context.Reservations, x => x.Id == model.Id);
                var Reservation = ReservationEntity.result;

                Reservation.Details = model.Details;
                Reservation.InitDate = model.InitDate;
                Reservation.EndDate = model.EndDate;
                Reservation.AgencyID = model.AgencyID;
                Reservation.RoomID = model.RoomID;
                Reservation.CheckIn = model.CheckIn;
                Reservation.CheckOut = model.CheckOut;
                
                await context.SaveChangesAsync();
            };
            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<Reservation>> RetrieveAsync(long id)
        {
            Func<Task<Reservation>> action = async () =>
            {
                IQueryable<Reservation> q = context.Reservations;
                q = SetIncludes(q);
                var result = await q.Where(x => x.Id == id).FirstAsync();
                return result;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> UpdateAsync(long id, Reservation model)
        {
            model.Id = id;

            Console.WriteLine("model.Id =" + model.Id.ToString());
            
            Func<Task> action = async () =>
            {
                model.InitDate = new DateTime(model.InitDate.Year, model.InitDate.Month, model.InitDate.Day, 9, 0, 0);
                model.EndDate = new DateTime(model.EndDate.Year, model.EndDate.Month, model.EndDate.Day, 8, 59, 59);

                if ( model.EndDate < model.InitDate ) {
                    throw new InvalidOperationException("La fecha de terminación no puede ser antes de la fecha de inicio");
                }

                IQueryable<Reservation> q = context.Reservations;
                q = q.Where(r => (r.Id != model.Id) && (r.RoomID == model.RoomID) && !(r.EndDate.CompareTo(model.InitDate) < 0 || model.EndDate.CompareTo(r.InitDate) < 0));
                int cnt = await q.CountAsync();
                if (cnt > 0)
                {
                    throw new InvalidOperationException("Existe un conflicto con otra reservación");
                }

                var ReservationEntity = await GetOrCreateEntityAsync(context.Reservations, x => x.Id == model.Id);
                var Reservation = ReservationEntity.result;

                Reservation.Details = model.Details;
                Reservation.InitDate = model.InitDate;
                Reservation.EndDate = model.EndDate;
                Reservation.AgencyID = model.AgencyID;
                Reservation.RoomID = model.RoomID;
                Reservation.CheckIn = model.CheckIn;
                Reservation.CheckOut = model.CheckOut;

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

        public async Task<ProcessResult> AddGuestAsync(long reservationId, long guestId) {
            Func<Task> action = async () =>
            {
                var GuestReservationEntity = await GetOrCreateEntityAsync(
                        context.GuestReservations,
                        x => x.GuestId == guestId && x.ReservationId == reservationId
                    );
                var GuestReservation = GuestReservationEntity.result;

                GuestReservation.GuestId = guestId;
                GuestReservation.ReservationId = reservationId;
                
                await context.SaveChangesAsync();
            };
            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> RemoveGuestAsync(long reservationId, long guestId) {
            Func<Task> action = async () =>
            {
                var o = await context.GuestReservations
                        .Where(x => x.GuestId == guestId && x.ReservationId == reservationId)
                        .SingleAsync();
                context.Remove(o);
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        private IQueryable<Reservation> SetIncludes(IQueryable<Reservation> q){
            q = q.Include( s => s.Agency );
            q = q.Include( s => s.Room );
            q = q.Include( s => s.GuestReservations )
                    .ThenInclude( gr => gr.Guest );
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
            if (f.agencyID > 0) {
                q = q.Where( s => s.AgencyID == f.agencyID );
            }
            if (f.roomID > 0) {
                q = q.Where( s => s.RoomID == f.roomID );
            }
            if ( f.checkInDate != DateTime.MinValue ) {
                DateTime tmp = new DateTime(f.checkInDate.Year, f.checkInDate.Month, f.checkInDate.Day, 9, 0, 0);
                q = q.Where( s => s.InitDate == tmp );
            }
            if ( f.checkOutDate != DateTime.MinValue ) {
                DateTime tmp = new DateTime(f.checkOutDate.Year, f.checkOutDate.Month, f.checkOutDate.Day, 8, 59, 59);
                q = q.Where( s => s.EndDate == tmp );
            }
            if ( f.InDate != DateTime.MinValue ) {
                DateTime tmp = new DateTime(f.InDate.Year, f.InDate.Month, f.InDate.Day, 9, 0, 0);
                q = q.Where( s => s.InitDate >= tmp );
            }
            if ( f.OutDate != DateTime.MinValue ) {
                DateTime tmp = new DateTime(f.OutDate.Year, f.OutDate.Month, f.OutDate.Day, 8, 59, 59);
                q = q.Where( s => s.EndDate <= tmp );
            }
            if ( f.checkInState != 0 ) {
                bool tmp = (f.checkInState == 1 ? true : false);
                q = q.Where( s => s.CheckIn == tmp );
            }

            if ( f.checkOutState != 0 ) {
                bool tmp = (f.checkOutState == 1 ? true : false);
                q = q.Where( s => s.CheckOut == tmp );
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