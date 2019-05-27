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
    public class CountryService : BaseService, ICountryService
    {
        public CountryService(UserManager<ApplicationUser> userManager,
                              IHttpContextAccessor contextAccessor,
                              ApplicationDbContext context)
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult> CreateAsync(Country model)
        {
            Func<Task> action = async () =>
            {

                var CountryExist = await context.Countries.Where(x => x.Name == model.Name).CountAsync();

                if (CountryExist > 0)
                {
                    throw new InvalidOperationException("Ya existe una país con ese nombre");
                }

                var CountryEntity = await GetOrCreateEntityAsync(context.Countries, x => x.Id == model.Id);
                var Country = CountryEntity.result;

                Country.Name = model.Name;

                await context.SaveChangesAsync();
            };
            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<Country>> RetrieveAsync(long id)
        {
            Func<Task<Country>> action = async () =>
            {
                var result = await context.Countries.Where(x => x.Id == id).FirstAsync();
                return result;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> UpdateAsync(long id, Country model)
        {
            model.Id = id;

            Func<Task> action = async () =>
            {
                var CountryExist = await context.Countries.Where(x => x.Name == model.Name && x.Id != model.Id).CountAsync();

                if (CountryExist > 0)
                {
                    throw new InvalidOperationException("Ya existe una país con ese nombre");
                }

                var CountryEntity = await GetOrCreateEntityAsync(context.Countries, x => x.Id == model.Id);
                var Country = CountryEntity.result;

                Country.Name = model.Name;

                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> DeleteAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var o = await context.Countries.Where(x => x.Id == id).SingleAsync();
                context.Countries.Remove(o);
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }


        public async Task<ProcessResult<List<Country>>> ListAsync(GetListViewModel<CountryFilter> getListModel)
        {
            IQueryable<Country> q = context.Countries;
            q = SetIncludes(q);
            q = SetFilter(q, getListModel.filter);

            var countItems = await q.CountAsync();

            q = SetPaginator(q, getListModel.paginator);
            q = SetOrderBy(q, getListModel.orderBy);

            Func<Task<List<Country>>> action = async () =>
            {
                var result = await q.ToListAsync();
                return result;
            };

            return await Process.RunAsync(action, countItems);
        }

        public async Task<ProcessResult<int>> CountAsync(CountryFilter filter)
        {
            IQueryable<Country> q = context.Countries;
            q = SetFilter(q, filter);

            Func<Task<int>> action = async () =>
            {
                var countItems = await q.CountAsync();
                return countItems;
            };

            return await Process.RunAsync(action);
        }

        private IQueryable<Country> SetIncludes(IQueryable<Country> q){
            return q;
        }

        private IQueryable<Country> SetOrderBy(IQueryable<Country> q, OrderBy ob) {
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

        private IQueryable<Country> SetFilter(IQueryable<Country> q, CountryFilter f) {
            if ( f == null ) {
                return q;
            }
            if (!String.IsNullOrEmpty(f.searchString))
            {
                q = q.Where(s => s.Name.Contains(f.searchString));
            }
            return q;
        }
    
        private IQueryable<Country> SetPaginator(IQueryable<Country> q, Paginator p) {
            if ( p == null ) {
                return q;
            }
            return q.Skip(p.offset).Take(p.limit);
        }
    }
}