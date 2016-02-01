using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using NLog;

namespace IdeasAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new NlogHandleErrorAttribute());
        }
    }

    public class NlogHandleErrorAttribute : HandleErrorAttribute
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override void OnException(ExceptionContext filterContext)
        {
            Logger.Error(filterContext.Exception);
            base.OnException(filterContext);
        }
    }
    public class NlogExceptionFilterAttribute : ExceptionFilterAttribute
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override void OnException(HttpActionExecutedContext context)
        {
            Logger.Error(context.Exception);
            base.OnException(context);
        }
    }
}
