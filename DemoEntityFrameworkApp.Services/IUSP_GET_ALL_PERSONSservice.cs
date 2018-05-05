using DemoEntityFrameworkApp.DataAccess.Interfaces;
using DemoEntityFrameworkApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEntityFrameworkApp.Services
{
    public interface IUSP_GET_ALL_PERSONSservice
    {
        List<USP_GET_ALL_PERSONS> GetAllPersonsFromDB(USP_GET_ALL_PERSONS per, Dictionary<object, object> parms);

    }
}
