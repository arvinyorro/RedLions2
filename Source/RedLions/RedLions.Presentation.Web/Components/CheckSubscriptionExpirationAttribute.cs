namespace RedLions.Presentation.Web.Components
{
    using System;
    using System.Web;
    using System.Web.Routing;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using RedLions.Application;
    using DTO = RedLions.Application.DTO;

    public class CheckSubscriptionExpirationAttribute : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            SubscriptionService subscriptionService = UnityConfig.GetConfiguredContainer().Resolve<SubscriptionService>();
            MemberService memberService = UnityConfig.GetConfiguredContainer().Resolve<MemberService>();

            HttpContextBase httpContext = filterContext.HttpContext;
            string username = httpContext.User.Identity.Name;

            DTO.Member memberDTO = memberService.GetMemberByUsername(username);

            bool subscriptionExpired = subscriptionService.HasSubscriptionExpired(memberDTO.ID);

            if (subscriptionExpired)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        {"controller", "Home"}, 
                        {"action", "Expired"}});
            }
        }
    }
}