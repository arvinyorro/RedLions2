using System;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using RedLions.Application;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Owin;

[assembly: OwinStartup(typeof(RedLions.Presentation.Web.Hubs.OwinStartup))]

namespace RedLions.Presentation.Web.Hubs
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver = new UnityDependencyResolver(UnityConfig.GetConfiguredContainer());
            app.MapSignalR();
        }
    }

    public class UnityDependencyResolver : DefaultDependencyResolver
    {
        private readonly IUnityContainer _container;

        public UnityDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }

        public override object GetService(Type serviceType)
        {
            if (_container.IsRegistered(serviceType))
            {
                return _container.Resolve(serviceType);
            }
            return base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            if (_container.IsRegistered(serviceType))
            {
                return _container.ResolveAll(serviceType);
            }
            return base.GetServices(serviceType);
        }
    }
}