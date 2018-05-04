using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DemoEntityFrameworkApp.DataAccess.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        List<T> SelectAll();
        T SelectByID(object id);
        void Add(T entity);
        void Edit(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(object id);
        void Save();
        List<T> Get_StoredProc(string Spname);
        List<T> Get_StoredProc_Name(T entity, Dictionary<object, object> parms);
        void void_sp(string spname, Dictionary<object, object> parms);

    }

}
