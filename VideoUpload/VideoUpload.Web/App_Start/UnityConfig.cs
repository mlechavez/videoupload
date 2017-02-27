using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using VideoUpload.EF;

namespace VideoUpload.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<UnitOfWork>(new InjectionConstructor("AppDbContext"));
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}