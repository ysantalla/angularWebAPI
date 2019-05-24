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

        public async Task<ProcessResult> CreateAsync(CurrencyViewModel model)
        {
            Func<Task> action = async () =>
            {

                var CurrencyExist = await context.Currencies.Where(x => x.Name == model.Name || x.Symbol == model.Symbol).CountAsync();

                if (CurrencyExist > 0)
                {
                    throw new InvalidOperationException("Ya existe una moneda con este nombre o con este símbolo");
                }

                var CurrencyEntity = await GetOrCreateEntityAsync(context.Currencies, x => x.Id == model.Id);
                var Currency = CurrencyEntity.result;

                Currency.Name = model.Name;
                Currency.Symbol = model.Symbol;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }


        public async Task<ProcessResult> UpdateAsync(CurrencyViewModel model)
        {

            Func<Task> action = async () =>
            {
                var CurrencyExist = await context.Currencies.Where(x => (x.Name == model.Name || x.Symbol == model.Symbol) && x.Id != model.Id).CountAsync();

                if (CurrencyExist > 0)
                {
                    throw new InvalidOperationException("Ya existe una moneda con este nombre o con este símbolo");
                }

                var CurrencyEntity = await GetOrCreateEntityAsync(context.Currencies, x => x.Id == model.Id);
                var Currency = CurrencyEntity.result;

                Currency.Name = model.Name;
                Currency.Symbol = model.Symbol;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> RemoveOrRestoreAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var Currency = await context.Currencies.Where(x => x.Id == id).SingleAsync();

                Currency.IsDeleted = !Currency.IsDeleted;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<Currency>> GetByIdAsync(long id)
        {
            Func<Task<Currency>> action = async () =>
            {
                var result = await context.Currencies.Where(x => x.Id == id).FirstAsync();

                var resultView = new Currency
                {
                    Id = result.Id,
                    Name = result.Name,
                    Symbol = result.Symbol
                };

                return resultView;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<List<Currency>>> GetListAsync(string sortOrder, string searchString, int pageIndex,  int pageSize)
        {
            IQueryable<Currency> CurrencyIQ = from s in context.Currencies select s;
            
            CurrencyIQ = CurrencyIQ.Where(s => s.IsDeleted == false);

            if (!String.IsNullOrEmpty(searchString))
            {
                CurrencyIQ = CurrencyIQ.Where(s => s.Name.Contains(searchString) && s.IsDeleted == false);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    CurrencyIQ = CurrencyIQ.OrderByDescending(s => s.Name);
                    break;
                case "name_asc":
                    CurrencyIQ = CurrencyIQ.OrderBy(s => s.Name);
                    break;
                case "id_desc":
                    CurrencyIQ = CurrencyIQ.OrderByDescending(s => s.Id);
                    break;
                case "id_asc":
                    CurrencyIQ = CurrencyIQ.OrderBy(s => s.Id);
                    break;
                default:
                    CurrencyIQ = CurrencyIQ.OrderBy(s => s.Name);
                    break;
            }

            var countItems = await CurrencyIQ.CountAsync();

            if (pageIndex != 0 && pageSize != 0) {
                CurrencyIQ = CurrencyIQ.Skip((pageIndex - 1) * pageSize).Take(pageSize);                
            }

            Func<Task<List<Currency>>> action = async () =>
            {
                var result = await CurrencyIQ.Select(x => new Currency
                {
                    Id = x.Id,
                    Name = x.Name,
                    Symbol = x.Symbol,
                    IsDeleted = x.IsDeleted
                }).ToListAsync();

                return result;
            };

            return await Process.RunAsync(action, countItems);
        }

        public async Task<ProcessResult<int>> CountAsync(string searchString = "")
        {
            IQueryable<Currency> CurrencyIQ = from s in context.Currencies select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                CurrencyIQ = CurrencyIQ.Where(s => s.Name.Contains(searchString) && s.IsDeleted == false);
            }           

            Func<Task<int>> action = async () =>
            {
                var countItems = await CurrencyIQ.CountAsync();

                return countItems;
            };

            return await Process.RunAsync(action);
        }

    }
}