using DemoEntityFrameworkApp.DataAccess.Interfaces;
using DemoEntityFrameworkApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEntityFrameworkApp.Services
{
    public class USP_GET_ALL_PERSONSservice:IUSP_GET_ALL_PERSONSservice
    {
        private readonly IUSP_GET_ALL_PERSONS _uSP_GET_ALL_PERSONS;

        public USP_GET_ALL_PERSONSservice(IUSP_GET_ALL_PERSONS USP_GET_ALL_PERSONS)
        {
            this._uSP_GET_ALL_PERSONS = USP_GET_ALL_PERSONS;
        }

        public List<USP_GET_ALL_PERSONS> GetAllPersonsFromDB(USP_GET_ALL_PERSONS per, Dictionary<object, object> parms)
        {

            return _uSP_GET_ALL_PERSONS.Get_StoredProc_Name(per, parms);
        }
    }
}
