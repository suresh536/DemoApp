using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject.Extensions.Conventions;

namespace DemoEntityFrameworkApp.App_Start
{
    public class NinjectControllerFactory:DefaultControllerFactory
    {
        private IKernel Kernel;
        public NinjectControllerFactory()
        {
            Kernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext,Type ControllerType)
        {
            return (ControllerType == null) ? null : (IController)Kernel.Get(ControllerType);
        }


        private void AddBindings()
        {
            Kernel.Bind(x => x.FromAssembliesMatching("DemoEntityFrameworkApp*.dll").SelectAllClasses().BindAllInterfaces());
        }
    }
}