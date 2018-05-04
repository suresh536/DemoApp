using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace DemoEntityFrameworkApp.Helpers
{
    public class AppSessionHandler : ActionFilterAttribute
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
        (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContextBase ctx = filterContext.HttpContext;
            if (ctx.Session["userid"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Index");
            }
            else
            {
                var controllerName = filterContext.RouteData.Values["controller"];
                var actionName = filterContext.RouteData.Values["action"];
                log.Info("Startup application. Controller Name: " + controllerName + "actionName: " + actionName);

            }
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];
            log.Info("Startup application. Controller Name: " + controllerName + "actionName: " + actionName);

            base.OnActionExecuted(filterContext);
        }

    }
}