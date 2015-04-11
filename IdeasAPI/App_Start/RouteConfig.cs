using System.Web.Mvc;
using System.Web.Routing;

namespace IdeasAPI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "Home", action = "Login" }
            );

            routes.MapRoute(
                name: "Logout",
                url: "logout",
                defaults: new { controller = "Home", action = "LogOut" }
            );

			routes.MapRoute(
				name: "Default",
				url: "{*pathInfo}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
        }
    }
}
