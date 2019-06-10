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
    public class InvoiceService : BaseService, IInvoiceService
    {
        public InvoiceService(UserManager<ApplicationUser> userManager,
                              IHttpContextAccessor contextAccessor,
                              ApplicationDbContext context)
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult> CreateAsync(Invoice model)
        {
            Func<Task> action = async () =>
            {
                var InvoiceEntity = await GetOrCreateEntityAsync(context.Invoices, x => x.Id == model.Id);
                var Invoice = InvoiceEntity.result;

                Invoice.Number = model.Number;
                Invoice.Date = model.Date;
                Invoice.ReservationID = model.ReservationID;
                Invoice.CurrencyID = model.CurrencyID;
                Invoice.State = model.State;

                await context.SaveChangesAsync();
            };
            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<Invoice>> RetrieveAsync(long id)
        {
            Func<Task<Invoice>> action = async () =>
            {
                var result = await context.Invoices.Where(x => x.Id == id).FirstAsync();
                return result;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> UpdateAsync(long id, Invoice model)
        {
            model.Id = id;

            Func<Task> action = async () =>
            {
                var InvoiceEntity = await GetOrCreateEntityAsync(context.Invoices, x => x.Id == model.Id);
                var Invoice = InvoiceEntity.result;

                Invoice.Number = model.Number;
                Invoice.Date = model.Date;
                Invoice.ReservationID = model.ReservationID;
                Invoice.CurrencyID = model.CurrencyID;
                Invoice.State = model.State;

                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> DeleteAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var o = await context.Invoices.Where(x => x.Id == id).SingleAsync();
                context.Remove(o);
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<List<Invoice>>> ListAsync(GetListViewModel<InvoiceFilter> getListModel)
        {
            IQueryable<Invoice> q = context.Invoices;
            q = SetIncludes(q);
            q = SetFilter(q, getListModel.filter);
            
            var countItems = await q.CountAsync();
            
            q = SetPaginator(q, getListModel.paginator);
            q = SetOrderBy(q, getListModel.orderBy);

            Func<Task<List<Invoice>>> action = async () =>
            {
                var result = await q.ToListAsync();
                return result;
            };

            return await Process.RunAsync(action, countItems);
        }

        public async Task<ProcessResult<int>> CountAsync(InvoiceFilter filter)
        {
            IQueryable<Invoice> q = context.Invoices;
            q = SetFilter(q, filter);

            Func<Task<int>> action = async () =>
            {
                var countItems = await q.CountAsync();
                return countItems;
            };

            return await Process.RunAsync(action);
        }

        private IQueryable<Invoice> SetIncludes(IQueryable<Invoice> q){
            q = q.Include( s => s.Reservation );
            q = q.Include( s => s.Currency );
            return q;
        }

        private IQueryable<Invoice> SetOrderBy(IQueryable<Invoice> q, OrderBy ob) {
            if ( ob == null ) {
                return q;
            }

            if ( !ob.desc ) {
                if ( ob.by == "number" ) {
                    q = q.OrderBy(s => s.Number);
                }
                else if ( ob.by == "date" ) {
                    q = q.OrderBy( s => s.Date );
                }
                else if ( ob.by == "state" ) {
                    q = q.OrderBy( s => s.State );
                }
                else {
                    q = q.OrderBy(s => s.Id);
                }
            }
            else {
                if ( ob.by == "number" ) {
                    q = q.OrderByDescending(s => s.Number);
                }
                else if ( ob.by == "date" ) {
                    q = q.OrderByDescending( s => s.Date );
                }
                else if ( ob.by == "state" ) {
                    q = q.OrderByDescending( s => s.State );
                } 
                else {
                    q = q.OrderByDescending(s => s.Id);
                }
            }
            return q;
        }

        private IQueryable<Invoice> SetFilter(IQueryable<Invoice> q, InvoiceFilter f) {
            if ( f == null ) {
                return q;
            }
            if (f.reservationID > 0) {
                q = q.Where( s => s.ReservationID == f.reservationID );
            }
            if (f.currencyID > 0) {
                q = q.Where( s => s.CurrencyID == f.currencyID );
            }
            if (f.Date != null) {
                q = q.Where( s => s.Date > f.Date );
            }
            return q;
        }
    
        private IQueryable<Invoice> SetPaginator(IQueryable<Invoice> q, Paginator p) {
            if ( p == null ) {
                return q;
            }
            return q.Skip(p.offset).Take(p.limit);
        }
    }
}