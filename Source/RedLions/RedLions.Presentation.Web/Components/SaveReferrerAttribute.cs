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

        /// <remarks>
        /// This routine needs more improvement in readability.
        /// </remarks>
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.ActionName == "ReferrerNotFound") return;

            MemberService memberService = UnityConfig.GetConfiguredContainer().Resolve<MemberService>();

            HttpContextBase httpContext = filterContext.HttpContext;

            // Get route data.
            var referrerRouteData = filterContext.RouteData.Values["referrerUsername"];
            bool referrerInUrl = referrerRouteData == null ? false : true;
            string parameterValue = referrerRouteData == null ? string.Empty : referrerRouteData.ToString();

            // Get referrer's username in cookie.
            HttpCookie cookie = httpContext.Request.Cookies.Get(this.referrerCookieName);
            bool referrerInCookie = (httpContext.Request.Cookies[this.referrerCookieName] != null);

            string referrerUsername = string.Empty;

            // If there an indicated referrer in the URL.
            if (referrerInUrl)
            {
                referrerUsername = parameterValue;
            }
            
            // If there is a referrer saved in the cookie and 
            // a referrer was not retrieved in the URL.
            if(referrerInCookie && string.IsNullOrEmpty(referrerUsername))
            {
                // Retrieve referrer in cookie.
                referrerUsername = cookie.Value;
            }
           
            // if referrer is still empty after checking url and cookie, then 
            // retrieve random username.
            if (string.IsNullOrEmpty(referrerUsername))
            {
                referrerUsername = memberService.GetRandomMember().Username;
            }

            if (string.IsNullOrEmpty(referrerUsername))
            {
                throw new Exception("Failed to generate a referrer.");
            }

            // verify
            var member = memberService.GetMemberByUsername(referrerUsername);
            bool referrerNotFound = member == null ? true : false;

            if (referrerNotFound)
            {
                // Generate random referrer as final solution.
                // referrerUsername = memberService.GetRandomMember().Username;

                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary 
                        { 
                            { "controller", "Home" }, 
                            { "action", "ReferrerNotFound" },
                        });
                return;
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