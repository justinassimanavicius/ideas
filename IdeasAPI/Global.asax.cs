using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using IdeasAPI.Code;
using IdeasAPI.Infrastructure;
using Microsoft.Practices.Unity.Mvc;
using NLog;

namespace IdeasAPI
{
    public class WebApiApplication : HttpApplication
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            UnityActivation.Activate();
            //MVC part
            DependencyResolver.SetResolver(new UnityDependencyResolver(UnityActivation.Container));
            //API part
            GlobalConfiguration.Configuration.DependencyResolver = new ApiUnityResolver(UnityActivation.Container);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Logger.Info("App started.");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            Logger.Error(ex);
        }
    }
}
