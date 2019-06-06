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
    public class GuestService : BaseService, IGuestService
    {
        public GuestService(UserManager<ApplicationUser> userManager,
                              IHttpContextAccessor contextAccessor,
                              ApplicationDbContext context)
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult> CreateAsync(Guest model)
        {
            Func<Task> action = async () =>
            {

                var GuestExist = await context.Guests.Where(x => x.Identification == model.Identification).CountAsync();

                if (GuestExist > 0)
                {
                    throw new InvalidOperationException("Ya existe un huésped con esa identificación");
                }

                var GuestEntity = await GetOrCreateEntityAsync(context.Guests, x => x.Id == model.Id);
                var Guest = GuestEntity.result;

                Guest.Name = model.Name;
                Guest.Phone = model.Phone;
                Guest.Identification = model.Identification;
                Guest.CountryID = model.CountryID;
                Guest.CitizenshipID = model.CitizenshipID;

                await context.SaveChangesAsync();
            };
            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<Guest>> RetrieveAsync(long id)
        {
            Func<Task<Guest>> action = async () =>
            {
                var result = await context.Guests
                                    .Where(x => x.Id == id).FirstAsync();
                return result;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> UpdateAsync(long id, Guest model)
        {
            model.Id = id;

            Func<Task> action = async () =>
            {
                var GuestExist = await context.Guests.Where(x => x.Name == model.Name && x.Id != model.Id).CountAsync();

                if (GuestExist > 0)
                {
                    throw new InvalidOperationException("Ya existe un huésped con esa identificación");
                }

                var GuestEntity = await GetOrCreateEntityAsync(context.Guests, x => x.Id == model.Id);
                var Guest = GuestEntity.result;

                Guest.Name = model.Name;
                Guest.Phone = model.Phone;
                Guest.Identification = model.Identification;
                Guest.CountryID = model.CountryID;
                Guest.CitizenshipID = model.CitizenshipID;

                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> DeleteAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var o = await context.Guests.Where(x => x.Id == id).SingleAsync();
                context.Remove(o);
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<List<Guest>>> ListAsync(GetListViewModel<GuestFilter> getListModel)
        {
            IQueryable<Guest> q = context.Guests;
            q = SetIncludes(q);
            q = SetFilter(q, getListModel.filter);
            
            var countItems = await q.CountAsync();

            q = SetPaginator(q, getListModel.paginator);
            q = SetOrderBy(q, getListModel.orderBy);

            Func<Task<List<Guest>>> action = async () =>
            {
                var result = await q.ToListAsync();
                return result;
            };

            return await Process.RunAsync(action, countItems);
        }

        public async Task<ProcessResult<int>> CountAsync(GuestFilter filter)
        {
            IQueryable<Guest> q = context.Guests;
            q = SetFilter(q, filter);

            Func<Task<int>> action = async () =>
            {
                var countItems = await q.CountAsync();
                return countItems;
            };

            return await Process.RunAsync(action);
        }

        private IQueryable<Guest> SetIncludes(IQueryable<Guest> q){
            q = q.Include(s => s.Country);
            q = q.Include(s => s.Citizenship);
            return q;
        }

        private IQueryable<Guest> SetOrderBy(IQueryable<Guest> q, OrderBy ob) {
            if ( ob == null ) {
                return q;
            }

            if ( !ob.desc ) {
                if ( ob.by == "name" ) {
                    q = q.OrderBy(s => s.Name);
                } 
                else {
                    q = q.OrderBy(s => s.Id);
                }
            }
            else {
                if ( ob.by == "name" ) {
                    q = q.OrderByDescending(s => s.Name);
                } 
                else {
                    q = q.OrderByDescending(s => s.Id);
                }
            }
            return q;
        }

        private IQueryable<Guest> SetFilter(IQueryable<Guest> q, GuestFilter f) {
            if ( f == null ) {
                return q;
            }
            if (!String.IsNullOrEmpty(f.searchString))
            {
                q = q.Where(s => s.Name.Contains(f.searchString));
            }
            if (f.identification != "") {
                q = q.Where(s => s.Identification == f.identification);
            }
            if (f.countryID != 0) {
                q = q.Where(s => s.CountryID == f.countryID);
            }
            if (f.citizenshipID != 0) {
                q = q.Where(s => s.CitizenshipID == f.citizenshipID);
            }
            return q;
        }
    
        private IQueryable<Guest> SetPaginator(IQueryable<Guest> q, Paginator p) {
            if ( p == null ) {
                return q;
            }
            return q.Skip(p.offset).Take(p.limit);
        }
    }
}