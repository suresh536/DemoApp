using DemoEntityFrameworkApp.DataAccess.Interfaces;
using DemoEntityFrameworkApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEntityFrameworkApp.DataAccess
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private SchoolDB db = null;
        private DbSet<T> table = null;

        public GenericRepository()
        {
            this.db = new SchoolDB();
            table = db.Set<T>();
        }

        public GenericRepository(SchoolDB db)
        {
            this.db = db;
            table = db.Set<T>();
        }

        public virtual IQueryable<T> GetAll()
        {

            IQueryable<T> query = db.Set<T>();
            return query;
        }

        public List<T> SelectAll()
        {
            return table.ToList();
        }

        public T SelectByID(object id)
        {
            return table.Find(id);
        }

        public void Add(T obj)
        {
            table.Add(obj);
        }

        public virtual void Edit(T entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            db.Entry(obj).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            db.Set<T>().Remove(entity);
        }

        public void Delete(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }


        public List<T> Get_StoredProc(string sPName)
        {
            return db.Database.SqlQuery<T>(sPName).ToList();
        }

        public List<T> Get_StoredProc_Name(T entity, Dictionary<object, object> parms)
        {
            Type typeParameterType = typeof(T);
            var spname = typeParameterType.Name + " ";
            var objParams = new SqlParameter[0];
            try
            {
                if(parms.Count>0)
                {
                    objParams = new SqlParameter[parms.Count];
                    int i = 0;
                    foreach (var item in parms)
                    {
                        SqlParameter param = new SqlParameter(item.Key.ToString(), item.Value);
                        objParams[i] = param;
                        if (i == 0)
                        {
                            spname += item.Key.ToString();
                        }
                        else
                        {
                            spname += "," + item.Key.ToString();
                        }
                        i++;
                    }

                }
            }
            catch(Exception ex)
            {

            }
            return db.Database.SqlQuery<T>(spname, objParams).ToList();
        }

        public void void_sp(string Procname, Dictionary<object, object> parms)
        {

            var spname = "EXEC " + Procname + "";
            var objParams = new SqlParameter[0];
            try
            {
                if (parms.Count > 0)
                {
                    objParams = new SqlParameter[parms.Count];
                    int i = 0;
                    foreach (var item in parms)
                    {
                        SqlParameter param = new SqlParameter(item.Key.ToString(), item.Value);
                        objParams[i] = param;
                        if (i == 0)
                        {
                            spname += item.Key.ToString();
                        }
                        else
                        {
                            spname += "," + item.Key.ToString();
                        }
                        i++;
                    }

                }
            }
            catch (Exception ex)
            {

            }
             db.Database.ExecuteSqlCommand(spname, objParams);
        }

        public virtual void Save()
        {
            db.SaveChanges();
        }

    }
}
