using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

using VideoUpload.Web.Models.Identity;
using VideoUpload.Core.Entities;
using VideoUpload.EF;

[assembly: OwinStartup(typeof(VideoUpload.Web.Startup))]
namespace VideoUpload.Web
{
    public class Startup
    {
        internal static IDataProtectionProvider DataProtectionProvider { get; private set; }
        public void Configuration(IAppBuilder app)
        {
            DataProtectionProvider = app.GetDataProtectionProvider();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookie",
                LoginPath = new PathString("/account/signin")
            });

            var mgr = DependencyResolver.Current.GetService(typeof(UserManager)) as UserManager;
            var store = DependencyResolver.Current.GetService(typeof(UnitOfWork)) as UnitOfWork;
            
            var user = mgr.FindByName("admin");

            if (user == null)
            {
                var u = new IdentityUser
                {
                    UserName = "lester",

                    FirstName = "Mark Lester",
                    LastName = "Echavez",
                    JobTitle = "IT Administrator",
                    EmployeeNo = "382",
                    IsActive = true,
                    Email = "echavez.marklester@boraq-porsche.com.qa",
                    EmailConfirmed = true,
                    EmailPass = "lester123"
                };
                var result = mgr.Create(u, "Lester@Dev");
                if (result.Succeeded)
                {                    
                    result = mgr.SetEmail(u.Id, u.Email);
                                        
                }                
            }
        }
    }
}