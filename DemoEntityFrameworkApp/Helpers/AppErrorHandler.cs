using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
namespace DemoEntityFrameworkApp.Helpers
{
    public class AppErrorHandler : HandleErrorAttribute
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
         (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override void OnException(ExceptionContext filterContext)
        {
            Exception e = filterContext.Exception;
            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult()
            {
                ViewName = "Error"
            };
            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];
            log.Info("Startup application. Controller Name: " + controllerName + "actionName: " + actionName + "Error : " + e.InnerException.ToString());
        }
    }
}