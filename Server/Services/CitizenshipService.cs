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
    public class CitizenshipService : BaseService, ICitizenshipService
    {
        public CitizenshipService(UserManager<ApplicationUser> userManager,
                              IHttpContextAccessor contextAccessor,
                              ApplicationDbContext context)
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult> CreateAsync(Citizenship model)
        {
            Func<Task> action = async () =>
            {

                var CitizenshipExist = await context.Citizenships.Where(x => x.Name == model.Name).CountAsync();

                if (CitizenshipExist > 0)
                {
                    throw new InvalidOperationException("Ya existe una ciudadanía con ese nombre");
                }

                var CitizenshipEntity = await GetOrCreateEntityAsync(context.Citizenships, x => x.Id == model.Id);
                var Citizenship = CitizenshipEntity.result;

                Citizenship.Name = model.Name;

                await context.SaveChangesAsync();
            };
            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<Citizenship>> RetrieveAsync(long id)
        {
            Func<Task<Citizenship>> action = async () =>
            {
                var result = await context.Citizenships.Where(x => x.Id == id).FirstAsync();
                return result;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> UpdateAsync(long id, Citizenship model)
        {
            model.Id = id;

            Func<Task> action = async () =>
            {
                var CitizenshipExist = await context.Citizenships.Where(x => x.Name == model.Name && x.Id != model.Id).CountAsync();

                if (CitizenshipExist > 0)
                {
                    throw new InvalidOperationException("Ya existe una ciudadanía con ese nombre");
                }

                var CitizenshipEntity = await GetOrCreateEntityAsync(context.Citizenships, x => x.Id == model.Id);
                var Citizenship = CitizenshipEntity.result;

                Citizenship.Name = model.Name;

                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> DeleteAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var Citizenship = await context.Citizenships.Where(x => x.Id == id).SingleAsync();

                Citizenship.IsDeleted = true;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> RestoreAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var Citizenship = await context.Citizenships.Where(x => x.Id == id).SingleAsync();

                Citizenship.IsDeleted = false;
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<List<Citizenship>>> ListAsync(GetListViewModel<CitizenshipFilter> getListModel)
        {
            IQueryable<Citizenship> q = context.Citizenships;
            q = SetIncludes(q);
            q = q.Where(s => !s.IsDeleted);
            q = SetFilter(q, getListModel.filter);
            q = SetPaginator(q, getListModel.paginator);
            q = SetOrderBy(q, getListModel.orderBy);

            Func<Task<List<Citizenship>>> action = async () =>
            {
                var result = await q.ToListAsync();
                return result;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<int>> CountAsync(CitizenshipFilter filter)
        {
            IQueryable<Citizenship> q = context.Citizenships;
            q = q.Where(s => !s.IsDeleted);
            q = SetFilter(q, filter);

            Func<Task<int>> action = async () =>
            {
                var countItems = await q.CountAsync();
                return countItems;
            };

            return await Process.RunAsync(action);
        }

        private IQueryable<Citizenship> SetIncludes(IQueryable<Citizenship> q){
            return q;
        }

        private IQueryable<Citizenship> SetOrderBy(IQueryable<Citizenship> q, OrderBy ob) {
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

        private IQueryable<Citizenship> SetFilter(IQueryable<Citizenship> q, CitizenshipFilter f) {
            if ( f == null ) {
                return q;
            }
            if (!String.IsNullOrEmpty(f.searchString))
            {
                q = q.Where(s => s.Name.Contains(f.searchString) && !s.IsDeleted);
            }
            return q;
        }
    
        private IQueryable<Citizenship> SetPaginator(IQueryable<Citizenship> q, Paginator p) {
            if ( p == null ) {
                return q;
            }
            return q.Skip(p.offset).Take(p.limit);
        }
    }
}