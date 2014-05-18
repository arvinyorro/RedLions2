using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(RedLions.Presentation.Web.Hubs.OwinStartup))]

namespace RedLions.Presentation.Web.Hubs
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}