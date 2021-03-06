﻿using ConcatPdf.Core.Implementations;
using ConcatPdf.Web.Controllers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ConcatPdf.Web
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/fwlink/?LinkId=301868
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RegisterDependency(GlobalConfiguration.Configuration);
        }

        private static void RegisterDependency(HttpConfiguration config)
        {
            var container = ContainerProvider.Current;
            config.DependencyResolver = new IoCContainer(container);
        }
    }
}
