using Microsoft.EntityFrameworkCore;
using ShopTech.DataAccess.Data;
using ShopTech.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ShopTech.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;

        internal DbSet<T> Set;
        public Repository(AppDbContext db)
        {
            _db  = db;
            Set = _db.Set<T>(); 
        }
        public IEnumerable<T>? GetAll(string TabelName ,string? filter = null, string? IncludePr = null)
        {
            IQueryable<T> query;
            if (filter != null)
            {
                query = Set.FromSqlRaw($"Select  TOP (100) PERCENT * from {TabelName} {filter}");
            }
            else
            {
                query = Set.FromSqlRaw($"SELECT  TOP (100) PERCENT * FROM dbo.{TabelName}");
            }

            if (!string.IsNullOrEmpty(IncludePr))
            {
                string[] Navigations = IncludePr.Split([',']);
                foreach(string Navigation in Navigations)
                {
                    query  = query.Include(Navigation);
                }
            }
            return query.ToList();
        }

        public T GetOne(string TabelName , string filter, string? IncludePr = null, bool Tracked = false)
        {
            var query = Set.FromSqlRaw($"Select * from {TabelName} {filter} ");

            if (!string.IsNullOrEmpty(IncludePr))
            {
                string[] Navigations = IncludePr.Split([',']);
                foreach (string Navigation in Navigations)
                {
                    query = query.Include(Navigation);
                }
            }
            if (!Tracked) query = query.AsNoTracking();
            return query.FirstOrDefault();
           
        }

        public void Remove(string TabelName ,int id)
        {
            _db.Database.ExecuteSqlRaw($"Delete From {TabelName} where Id = {id}");
        }
    }
}
