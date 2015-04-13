using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ideas.BusinessLogic.Infrastructure;
using IdeasAPI.Code;
using Microsoft.Practices.Unity.Mvc;

namespace IdeasAPI
{
    public class WebApiApplication : HttpApplication
    {
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
        }
    }
}
