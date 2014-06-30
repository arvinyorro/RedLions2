namespace RedLions.Presentation.Web
{
    using System.Web.Optimization;

    public class BundleConfig
    {
       // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.custom.js",
                        "~/Scripts/jquery.ui.*")); // Required by Metro

            bundles.Add(new StyleBundle("~/bundles/mvc-custom")
                .Include("~/Content/Styles/mvc-custom.css"));

            bundles.Add(new StyleBundle("~/bundles/homepage-ui")
                .Include("~/Content/Styles/homepage-ui.css"));

            bundles.Add(new StyleBundle("~/bundles/profile-ui")
                .Include("~/Content/Styles/profile-ui.css"));

            /* Begin Metro */
           
            bundles.Add(new StyleBundle("~/bundles/metro-style")
                .Include(
                "~/Content/Styles/Metro/metro-custom.css", // Custom CSS
                "~/Content/Styles/Metro/metro-bootstrap.css",
                "~/Content/Styles/Metro/iconFont.css"));

            //bundles.Add(new ScriptBundle("~/bundles/metro-script")
            //    .Include("~/Scripts/Metro/metro-*"));

            bundles.Add(new ScriptBundle("~/bundles/metro-script")
                .Include("~/Scripts/Metro/metro.min.js"));

            /* End Metro */

            /* Begin Responsive Slides */

            bundles.Add(new StyleBundle("~/bundles/responsive-slides-style")
                .Include("~/Content/Styles/ResponsiveSlides/responsive-slides.css"));

            bundles.Add(new ScriptBundle("~/bundles/responsiveslides")
                .Include("~/Scripts/responsiveslides.js"));

            /* End Responsive Slides */


            // SignalR
            bundles.Add(new ScriptBundle("~/bundles/signalr")
                .Include("~/Scripts/jquery.signalR-{version}.js"));

            // Clamp
            bundles.Add(new ScriptBundle("~/bundles/clamp")
                .Include("~/Scripts/clamp.js"));
        }
    }
}