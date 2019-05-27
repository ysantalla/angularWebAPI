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
    public class PackageService : BaseService, IPackageService
    {
        public PackageService(UserManager<ApplicationUser> userManager,
                              IHttpContextAccessor contextAccessor,
                              ApplicationDbContext context)
            : base(userManager, contextAccessor, context)
        {
        }

        public async Task<ProcessResult> CreateAsync(Package model)
        {
            Func<Task> action = async () =>
            {

                var PackageExist = await context.Packages.Where(x => x.Name == model.Name).CountAsync();

                if (PackageExist > 0)
                {
                    throw new InvalidOperationException("Ya existe un pquete con el mismo nombre");
                }

                var PackageEntity = await GetOrCreateEntityAsync(context.Packages, x => x.Id == model.Id);
                var Package = PackageEntity.result;

                Package.Name = model.Name;
                Package.Description = model.Description;
                Package.Value = model.Value;

                await context.SaveChangesAsync();
            };
            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<Package>> RetrieveAsync(long id)
        {
            Func<Task<Package>> action = async () =>
            {
                var result = await context.Packages.Where(x => x.Id == id).FirstAsync();
                return result;
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> UpdateAsync(long id, Package model)
        {
            model.Id = id;

            Func<Task> action = async () =>
            {
                var PackageExist = await context.Packages.Where(x => x.Name == model.Name && x.Id != model.Id).CountAsync();

                if (PackageExist > 0)
                {
                    throw new InvalidOperationException("Ya existe una paquete con el mismo nombre");
                }

                var PackageEntity = await GetOrCreateEntityAsync(context.Packages, x => x.Id == model.Id);
                var Package = PackageEntity.result;

                Package.Name = model.Name;
                Package.Description = model.Description;
                Package.Value = model.Value;

                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult> DeleteAsync(long id)
        {
            Func<Task> action = async () =>
            {
                var o = await context.Packages.Where(x => x.Id == id).SingleAsync();
                context.Remove(o);
                await context.SaveChangesAsync();
            };

            return await Process.RunAsync(action);
        }

        public async Task<ProcessResult<List<Package>>> ListAsync(GetListViewModel<PackageFilter> getListModel)
        {
            IQueryable<Package> q = context.Packages;
            q = SetIncludes(q);
            q = SetFilter(q, getListModel.filter);

            var countItems = await q.CountAsync();

            q = SetPaginator(q, getListModel.paginator);
            q = SetOrderBy(q, getListModel.orderBy);

            Func<Task<List<Package>>> action = async () =>
            {
                var result = await q.ToListAsync();
                return result;
            };

            return await Process.RunAsync(action, countItems);
        }

        public async Task<ProcessResult<int>> CountAsync(PackageFilter filter)
        {
            IQueryable<Package> q = context.Packages;
            q = SetFilter(q, filter);

            Func<Task<int>> action = async () =>
            {
                var countItems = await q.CountAsync();
                return countItems;
            };

            return await Process.RunAsync(action);
        }

        private IQueryable<Package> SetIncludes(IQueryable<Package> q){
            return q;
        }

        private IQueryable<Package> SetOrderBy(IQueryable<Package> q, OrderBy ob) {
            if ( ob == null ) {
                return q;
            }

            if ( !ob.desc ) {
                if ( ob.by == "name" ) {
                    q = q.OrderBy(s => s.Name);
                }
                if ( ob.by == "value" ) {
                    q = q.OrderBy(s => s.Value);
                } 
                else {
                    q = q.OrderBy(s => s.Id);
                }
            }
            else {
                if ( ob.by == "name" ) {
                    q = q.OrderByDescending(s => s.Name);
                } 
                if ( ob.by == "value" ) {
                    q = q.OrderByDescending(s => s.Value);
                }
                else {
                    q = q.OrderByDescending(s => s.Id);
                }
            }
            return q;
        }

        private IQueryable<Package> SetFilter(IQueryable<Package> q, PackageFilter f) {
            if ( f == null ) {
                return q;
            }
            if (!String.IsNullOrEmpty(f.searchString))
            {
                q = q.Where(s => (
                    s.Name.Contains(f.searchString) ||
                    s.Description.Contains(f.searchString) ||
                    s.Value.Contains(f.searchString)
                ));
            }
            return q;
        }
    
        private IQueryable<Package> SetPaginator(IQueryable<Package> q, Paginator p) {
            if ( p == null ) {
                return q;
            }
            return q.Skip(p.offset).Take(p.limit);
        }
    }
}