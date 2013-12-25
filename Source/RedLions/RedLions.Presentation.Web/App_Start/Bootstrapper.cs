namespace RedLions.Presentation.Web
{
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Mvc;
    // Other Layers
    using RedLions.Application;
    using RedLions.Infrastructure.Repository;
    using RedLions.Business;
    using RedLions.CrossCutting;

    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();    
            RegisterTypes(container);
            // Register the relevant types for the 
            // container here through classes or configuration
            container.RegisterType<RedLionsContext, RedLionsContext>(new PerRequestLifetimeManager());
            container.RegisterType<IRepository, GenericRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IMemberRepository, MemberRepository>();
            container.RegisterType<MemberService, MemberService>();
            container.RegisterType<UserService, UserService>();

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

        }
    }
}