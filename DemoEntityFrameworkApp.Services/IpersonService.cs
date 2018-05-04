using DemoEntityFrameworkApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEntityFrameworkApp.Services
{
    public interface IpersonService
    {
        List<Person> GetAllPersons();
    }
}
