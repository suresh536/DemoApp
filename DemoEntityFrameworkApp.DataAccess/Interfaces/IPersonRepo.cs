using DemoEntityFrameworkApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEntityFrameworkApp.DataAccess.Interfaces
{
    public interface IPersonRepo : IGenericRepository<Person>
    {
        Person GetSingle(int fooId);
    }

    public class PersonRepo : GenericRepository<Person>, IPersonRepo
    {
        public Person GetSingle(int Id)
        {
            var query = SelectByID(Id);
            return query;
        }
    }
}