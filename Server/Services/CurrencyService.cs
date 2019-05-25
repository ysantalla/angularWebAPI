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
    public class CurrencyService : BaseService, ICurrencyService
    {
        public CurrencyService(UserManager<ApplicationUser> userManager,
                              IHttpContextAccessor contextAccessor,
                              ApplicationDbContext context)
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult> CreateAsync(Currency model)
        {
            Func<Task> action = async () =>
            {

                var CurrencyExist = await context.Currencies.Where(x => x.Name == model.Name || x.Symbol == model.Symbol).CountAsync();

                if (CurrencyExist > 0)
                {
                    throw new InvalidOperationException("Ya existe una moneda con ese nombre o con ese símbolo");
                }

                var CurrencyEntity = await GetOrCreateEntityAsync(context.Currencies, x => x.Id == model.Id);
                var Currency = CurrencyEntity.result;

                Currency.Name = model.Name;
                Currency.Symbol = model.Symbol;

                await context.SaveChangesAsync();
            };
            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<Currency>> RetrieveAsync(long id)
        {
            Func<Task<Currency>> action = async () =>
            {
                var result = await context.Currencies.Where(x => x.Id == id).FirstAsync();
                return result;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> UpdateAsync(long id, Currency model)
        {
            model.Id = id;

            Func<Task> action = async () =>
            {
                var CurrencyExist = await context.Currencies.Where(x => (x.Name == model.Name || x.Symbol == model.Symbol) && x.Id != model.Id).CountAsync();

                if (CurrencyExist > 0)
                {
                    throw new InvalidOperationException("Ya existe una moneda con ese nombre o con ese símbolo");
                }

                var CurrencyEntity = await GetOrCreateEntityAsync(context.Currencies, x => x.Id == model.Id);
                var Currency = CurrencyEntity.result;

                Currency.Name = model.Name;
                Currency.Symbol = model.Symbol;

                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> DeleteAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var Currency = await context.Currencies.Where(x => x.Id == id).SingleAsync();

                Currency.IsDeleted = true;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> RestoreAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var Currency = await context.Currencies.Where(x => x.Id == id).SingleAsync();

                Currency.IsDeleted = false;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<List<Currency>>> ListAsync(GetListViewModel<CurrencyFilter> getListModel)
        {
            IQueryable<Currency> q = context.Currencies;
            q = SetIncludes(q);
            q = q.Where(s => !s.IsDeleted);
            q = SetFilter(q, getListModel.filter);
            q = SetPaginator(q, getListModel.paginator);
            q = SetOrderBy(q, getListModel.orderBy);

            Func<Task<List<Currency>>> action = async () =>
            {
                var result = await q.ToListAsync();
                return result;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<int>> CountAsync(CurrencyFilter filter)
        {
            IQueryable<Currency> q = context.Currencies;
            q = q.Where(s => !s.IsDeleted);
            q = SetFilter(q, filter);

            Func<Task<int>> action = async () =>
            {
                var countItems = await q.CountAsync();
                return countItems;
            };

            return await Process.RunAsync(action);
        }

        private IQueryable<Currency> SetIncludes(IQueryable<Currency> q){
            return q;
        }

        private IQueryable<Currency> SetOrderBy(IQueryable<Currency> q, OrderBy ob) {
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

        private IQueryable<Currency> SetFilter(IQueryable<Currency> q, CurrencyFilter f) {
            if ( f == null ) {
                return q;
            }
            if (!String.IsNullOrEmpty(f.searchString))
            {
                q = q.Where(s => (s.Name.Contains(f.searchString) || s.Symbol.Contains(f.searchString)) && !s.IsDeleted);
            }
            return q;
        }
    
        private IQueryable<Currency> SetPaginator(IQueryable<Currency> q, Paginator p) {
            if ( p == null ) {
                return q;
            }
            return q.Skip(p.offset).Take(p.limit);
        }
    }
}