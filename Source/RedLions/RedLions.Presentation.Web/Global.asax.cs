namespace RedLions.Presentation.Web
{
    using System.Linq;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    // Internals
    using RedLions.Presentation.Web.Components;
    using AppLayer = RedLions.Application;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            // Add support for the /Partials directory for partial views
            ViewEngines.Engines.Clear();
            var viewEngine = new RazorViewEngine();
            viewEngine.PartialViewLocationFormats = (new[]
                                    {
                                        "~/Views/{1}/Shared/{0}.cshtml",
                                    }).Concat(viewEngine.PartialViewLocationFormats).ToArray();
            ViewEngines.Engines.Add(viewEngine);

            //Bootstrapper.Initialise(); // Unity IoC integration

            AppLayer.AutoMapperConfig.Register();
        }
    }
}