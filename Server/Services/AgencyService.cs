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
    public class AgencyService : BaseService, IAgencyService
    {
        public AgencyService(UserManager<ApplicationUser> userManager,
                              IHttpContextAccessor contextAccessor,
                              ApplicationDbContext context)
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult> CreateAsync(Agency model)
        {
            Func<Task> action = async () =>
            {

                var AgencyExist = await context.Agencies.Where(x => x.Name == model.Name || x.Email == model.Email).CountAsync();

                if (AgencyExist > 0)
                {
                    throw new InvalidOperationException("Ya existe una agencia con ese nombre o con el mismo correo");
                }

                var AgencyEntity = await GetOrCreateEntityAsync(context.Agencies, x => x.Id == model.Id);
                var Agency = AgencyEntity.result;

                Agency.Name = model.Name;
                Agency.Represent = model.Represent;
                Agency.CountryID = model.CountryID;
                Agency.Email = model.Email;
                Agency.Phone = model.Phone;

                await context.SaveChangesAsync();
            };
            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<Agency>> RetrieveAsync(long id)
        {
            Func<Task<Agency>> action = async () =>
            {
                var result = await context.Agencies.Where(x => x.Id == id).FirstAsync();
                return result;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> UpdateAsync(long id, Agency model)
        {
            model.Id = id;

            Func<Task> action = async () =>
            {
                var AgencyExist = await context.Agencies.Where(x => (x.Name == model.Name || x.Email == model.Email) && x.Id != model.Id).CountAsync();

                if (AgencyExist > 0)
                {
                    throw new InvalidOperationException("Ya existe una agencia con el mismo nombre o con el mismo correo");
                }

                var AgencyEntity = await GetOrCreateEntityAsync(context.Agencies, x => x.Id == model.Id);
                var Agency = AgencyEntity.result;

                Agency.Name = model.Name;
                Agency.Represent = model.Represent;
                Agency.CountryID = model.CountryID;
                Agency.Email = model.Email;
                Agency.Phone = model.Phone;

                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> DeleteAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var Agency = await context.Agencies.Where(x => x.Id == id).SingleAsync();

                Agency.IsDeleted = true;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> RestoreAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var Agency = await context.Agencies.Where(x => x.Id == id).SingleAsync();

                Agency.IsDeleted = false;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<List<Agency>>> ListAsync(GetListViewModel<AgencyFilter> getListModel)
        {
            IQueryable<Agency> q = context.Agencies;
            q = SetIncludes(q);
            q = q.Where(s => !s.IsDeleted);
            q = SetFilter(q, getListModel.filter);
            q = SetPaginator(q, getListModel.paginator);
            q = SetOrderBy(q, getListModel.orderBy);

            Func<Task<List<Agency>>> action = async () =>
            {
                var result = await q.ToListAsync();
                return result;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<int>> CountAsync(AgencyFilter filter)
        {
            IQueryable<Agency> q = context.Agencies;
            q = q.Where(s => !s.IsDeleted);
            q = SetFilter(q, filter);

            Func<Task<int>> action = async () =>
            {
                var countItems = await q.CountAsync();
                return countItems;
            };

            return await Process.RunAsync(action);
        }

        private IQueryable<Agency> SetIncludes(IQueryable<Agency> q){
            q = q.Include( s => s.Country );
            return q;
        }

        private IQueryable<Agency> SetOrderBy(IQueryable<Agency> q, OrderBy ob) {
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

        private IQueryable<Agency> SetFilter(IQueryable<Agency> q, AgencyFilter f) {
            if ( f == null ) {
                return q;
            }
            if (!String.IsNullOrEmpty(f.searchString))
            {
                q = q.Where(s => (
                    s.Name.Contains(f.searchString) ||
                    s.Represent.Contains(f.searchString) ||
                    s.Email.Contains(f.searchString) ||
                    s.Phone.Contains(f.searchString)
                ) && !s.IsDeleted);
            }
            if (f.countryID > 0) {
                q = q.Where( s => s.CountryID == f.countryID );
            }
            return q;
        }
    
        private IQueryable<Agency> SetPaginator(IQueryable<Agency> q, Paginator p) {
            if ( p == null ) {
                return q;
            }
            return q.Skip(p.offset).Take(p.limit);
        }
    }
}