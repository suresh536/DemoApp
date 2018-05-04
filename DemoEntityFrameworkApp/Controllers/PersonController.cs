using DemoEntityFrameworkApp.Common;
using DemoEntityFrameworkApp.Models;
using DemoEntityFrameworkApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoEntityFrameworkApp.Controllers
{
    public class PersonController : Controller
    {
        private readonly IpersonService _personService;
        public PersonController(IpersonService PersonService)
        {
            this._personService = PersonService;
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
                draw = Conversion.ToInt32(Request.Form.GetValues("draw").FirstOrDefault()),
                start = Request.Form.GetValues("start").FirstOrDefault().ToInt32(),
                length = Request.Form.GetValues("length").FirstOrDefault().ToInt32(),
                search = Request.Form.GetValues("search").FirstOrDefault().ToStr(),
                sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault(),
                sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault()
            };
            Dictionary<object, object> dic = new Dictionary<object, object>();
            dic.Add("@pageNo", objpagedlist.draw);
            dic.Add("@pagesize", objpagedlist.length);
            if(string.IsNullOrEmpty(objpagedlist.search))
            {
                dic.Add("@Search", DBNull.Value);
            }
            else
            {
                dic.Add("@Search", objpagedlist.search);
            }
            dic.Add("@SortColumn", objpagedlist.sortColumn);
            dic.Add("@SortOrder", objpagedlist.sortColumnDir);

            var list = new List<Person>();
            int totalrecords = 0;
            return Json(new { draw = objpagedlist.draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = list },JsonRequestBehavior.AllowGet);
        }
    }
}