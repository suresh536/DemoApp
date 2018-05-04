using DemoEntityFrameworkApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DemoEntityFrameworkApp.Common
{
    public class SessionManager
    {
        public static Course CourseInfo
        {
            get
            {
                if (HttpContext.Current.Session["Course"] != null)
                    return HttpContext.Current.Session["Course"] as Course;
                else
                    return null;
               
            }
            set
            {
                HttpContext.Current.Session["Course"] = value;
            }

        }

        public static List<Department> DepartmentInfo
        {
            get
            {
                if (HttpContext.Current.Session["Department"] != null)
                    return HttpContext.Current.Session["Department"] as List<Department>;
                else
                    return null;

            }
            set
            {
                HttpContext.Current.Session["Department"] = value;
            }

        }
    }
}
