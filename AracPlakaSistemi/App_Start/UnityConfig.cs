using AracPlakaSistemi.Controllers.Abstract;
using AracPlakaSistemi.Infrastructure;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace AracPlakaSistemi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<ICacheService, InMemoryCache>();
            container.RegisterType<AdminBaseController>(new InjectionConstructor());



            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}