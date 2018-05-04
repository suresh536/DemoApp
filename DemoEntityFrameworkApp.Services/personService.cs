using DemoEntityFrameworkApp.DataAccess.Interfaces;
using DemoEntityFrameworkApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEntityFrameworkApp.Services
{
    public class personService : IpersonService
    {
        private readonly IPersonRepo _person;
        public personService(IPersonRepo Person)
        {
            this._person = Person;
        }

        public List<Person> GetAllPersons()
        {
            return _person.SelectAll().ToList();
        }
    }
}
