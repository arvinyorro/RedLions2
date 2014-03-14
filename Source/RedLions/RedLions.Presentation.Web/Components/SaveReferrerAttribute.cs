namespace RedLions.Presentation.Web.Components
{
    using System.Web;
    using System.Web.Routing;
    using System.Web.Mvc;

    public class SaveReferrerAttribute : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContextBase httpContext = filterContext.HttpContext;
            RouteData routeData = httpContext.Request.RequestContext.RouteData;

            object parameterValue;
            bool parameterNotFound = !routeData.Values.TryGetValue(
                "referrerUsername", 
                out parameterValue);

            if (parameterNotFound)
            {
                return;
            }

            httpContext.Session["ReferrerUsername"] = parameterValue as string;
        }
    }
}