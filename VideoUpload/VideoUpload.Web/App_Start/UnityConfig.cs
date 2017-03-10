using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using VideoUpload.EF;
using VideoUpload.Core;
using Microsoft.AspNet.Identity;
using VideoUpload.Web.Models.Identity;

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

			container.RegisterType<IUnitOfWork, UnitOfWork>(new InjectionConstructor("AppDbContext"));
			container.RegisterType<IUserStore<IdentityUser, string>, UserStore>(new TransientLifetimeManager());
						
			DependencyResolver.SetResolver(new UnityDependencyResolver(container));
		}
	}
}