using DemoEntityFrameworkApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEntityFrameworkApp.DataAccess.Interfaces
{
    public interface IUSP_GET_ALL_PERSONS : IGenericRepository<USP_GET_ALL_PERSONS>
    {

    }

    public class USP_GET_ALL_PERSONSS : GenericRepository<USP_GET_ALL_PERSONS>, IUSP_GET_ALL_PERSONS
    {
    }
}
