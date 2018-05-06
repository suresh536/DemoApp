using DemoEntityFrameworkApp.Common;
using DemoEntityFrameworkApp.DataAccess;
using DemoEntityFrameworkApp.Models;
using DemoEntityFrameworkApp.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace DemoEntityFrameworkApp.Controllers
{
    public class PersonController : Controller
    {
        DBCommon objCommon = new DBCommon();
        private readonly IpersonService _personService;
        private readonly IUSP_GET_ALL_PERSONSservice _USP_GET_ALL_PERSONSservice;
        public PersonController(IpersonService PersonService, IUSP_GET_ALL_PERSONSservice USP_GET_ALL_PERSONSservice)
        {
            this._personService = PersonService;
            this._USP_GET_ALL_PERSONSservice = USP_GET_ALL_PERSONSservice;
        }

        private string ConvertDate(string dates)
        {
            string a = dates;
            String[] str = new String[3];
            str = a.Split('/');
            String date = String.Empty;
            date = str[0];
            str[0] = str[1];
            str[1] = date;
            date = str[0] + "/" + str[1] + "/" + str[2];
            return date;
        }

        // GET: Person
        public ActionResult Index()
        {
            var data = _personService.GetAllPersons();
            return View(data);
        }

        public ActionResult GetAllPersonsWithPaging()
        {
            pagedlist objpagedlist = new pagedlist()
            {
                draw = Request.Form.GetValues("draw").FirstOrDefault().ToInt32(),
                start = Convert.ToInt32(Request.Params["start"]),
            length = Request.Form.GetValues("length").FirstOrDefault().ToInt32(),
                search = Request.Form.GetValues("search[value]").FirstOrDefault().ToStr(),
                sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault(),
                sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault()
            };
            Dictionary<object, object> dic = new Dictionary<object, object>();
           
            dic.Add("@PageNo",  objpagedlist.start);
            dic.Add("@PageSize", objpagedlist.length);
            if (string.IsNullOrEmpty(objpagedlist.search))
            {
                dic.Add("@Search", DBNull.Value);
            }
            else
            {
                dic.Add("@Search", objpagedlist.search);
            }
            dic.Add("@SortColumn", objpagedlist.sortColumn);
            dic.Add("@SortOrder", objpagedlist.sortColumnDir);

            //Hashtable ht = new Hashtable();
            //ht.Add("PageNo", objpagedlist.start==0?1: objpagedlist.start);
            //ht.Add("PageSize", objpagedlist.length);
            //ht.Add("Search", objpagedlist.search==null?DBNull.Value:(object)objpagedlist.search);
            //ht.Add("SortColumn", objpagedlist.sortColumn);
            //ht.Add("SortOrder", objpagedlist.sortColumnDir);
            //DataTable dt = objCommon.TableforSP("USP_GET_ALL_PERSONS", ht);
            //List<USP_GET_ALL_PERSONS> list = new List<USP_GET_ALL_PERSONS>();
            //list = (from DataRow row in dt.Rows

            //        select new USP_GET_ALL_PERSONS()
            //        {
            //            LastName = row["LastName"].ToString(),
            //            FirstName = row["FirstName"].ToString(),
            //            HireDate = row["HireDate"].ToStr(),
            //            EnrollmentDate = row["EnrollmentDate"].ToString(),
            //            totalcount = row["totalcount"].ToInt32()
            //        }).ToList();

            var list = _USP_GET_ALL_PERSONSservice.GetAllPersonsFromDB(new USP_GET_ALL_PERSONS(), dic);
            int totalrecords = list.Select(x => x.totalcount).FirstOrDefault();

            return Json(new { draw = objpagedlist.draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = list }, JsonRequestBehavior.AllowGet);
        }


}
}