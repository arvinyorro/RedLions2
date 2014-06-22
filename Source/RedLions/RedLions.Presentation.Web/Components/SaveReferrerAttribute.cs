﻿namespace RedLions.Presentation.Web.Components
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

            string referrerUsername = string.Empty;

            if (referrerInUrl)
            {
                // Retrieve referrer in Url query string.
                referrerUsername = parameterValue as string;
            }
            else if(referrerInCookie)
            {
                // Retrieve referrer in cookie.
                referrerUsername = cookie.Value;
            }
            else
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

            httpContext.Session["ReferrerUsername"] = referrerUsername;
        }
    }
}