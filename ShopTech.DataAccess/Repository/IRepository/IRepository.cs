using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTech.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string TableName,string? filter = null, string? IncludePr = null);
        T GetOne(string TabelName,string filter , string? IncludePr = null, bool Tracked = false);

        void Remove(string TableName, int id);
    }
}
