namespace RedLions.Presentation.Web.Components
{
    using System;
    using System.Web;
    using System.Web.Routing;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using RedLions.Application;

    public class SaveReferrerAttribute : ActionFilterAttribute, IActionFilter
    {
        private string referrerCookieName = "ReferrerUsername";

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            MemberService memberService = UnityConfig.GetConfiguredContainer().Resolve<MemberService>();

            HttpContextBase httpContext = filterContext.HttpContext;
            
            // Retrieve referrer's username in the query string.
            object parameterValue;
            RouteData routeData = httpContext.Request.RequestContext.RouteData;
            bool referrerInUrl = routeData.Values.TryGetValue(
                "referrerUsername", 
                out parameterValue);

            // Get referrer's username in cookie.
            HttpCookie cookie = httpContext.Request.Cookies.Get(this.referrerCookieName);
            bool referrerInCookie = (httpContext.Request.Cookies[this.referrerCookieName] != null);

            HttpCookie referrerUrlCookie = httpContext.Request.Cookies.Get("ReferrerUrl");
            bool referrerUrlInCookie = (httpContext.Request.Cookies["ReferrerUrl"] != null);

            string referrerUsername = string.Empty;

            if (referrerInUrl)
            {
                string value = parameterValue as string;

                if (referrerUrlInCookie && value != referrerUrlCookie.Value)
                {
                    // Retrieve referrer in Url query string.
                    referrerUsername = value;
                }
            }
            else if(referrerInCookie)
            {
                // Retrieve referrer in cookie.
                referrerUsername = cookie.Value;
            }

            bool referrerNotFound = false;
            if (referrerUsername != string.Empty)
            {
                referrerNotFound = memberService.GetMemberByUsername(referrerUsername) == null ? true : false;
            }

            if (string.IsNullOrEmpty(referrerUsername) || referrerNotFound)
            {
                // Generate random referrer as final solution.
                referrerUsername = memberService.GetRandomMember().Username;
            }

            if (string.IsNullOrEmpty(referrerUsername))
            {
                throw new Exception("Failed to generate a referrer.");
            }

            // Create new cookie.
            var newCookie = new HttpCookie(this.referrerCookieName);
            newCookie.Value = referrerUsername;
            newCookie.Expires = DateTime.Now.AddDays(30);
            httpContext.Response.Cookies.Set(newCookie);

            if (referrerUrlInCookie && referrerUrlCookie.Value != parameterValue as string)
            {
                // Create new cookie.
                var newUrlReferrerCookie = new HttpCookie("ReferrerUrl");
                newUrlReferrerCookie.Value = parameterValue as string;
                newUrlReferrerCookie.Expires = DateTime.Now.AddDays(30);
                httpContext.Response.Cookies.Set(newUrlReferrerCookie);
            }
            
            httpContext.Session["ReferrerUsername"] = referrerUsername;
        }
    }
}