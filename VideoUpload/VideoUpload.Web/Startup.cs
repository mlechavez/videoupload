using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

using VideoUpload.Web.Models.Identity;
using VideoUpload.Core.Entities;
using VideoUpload.EF;
using System.Collections.Generic;
using System.Security.Claims;

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
            var unitOfWork = DependencyResolver.Current.GetService(typeof(UnitOfWork)) as UnitOfWork;

            var activities = unitOfWork.Activities.GetAll();

            if (activities.Count == 0)
            {                
                activities.Add(new Activity { Type = "ManageUser", Value = "CanCreate" });
                activities.Add(new Activity { Type = "ManageUser", Value = "CanRead" });
                activities.Add(new Activity { Type = "ManageUser", Value = "CanUpdate" });
                activities.Add(new Activity { Type = "ManageUser", Value = "CanDelete" });
                activities.Add(new Activity { Type = "ManageUser", Value = "CanManageClaims" });
                activities.Add(new Activity { Type = "Activities", Value = "CanCreate" });
                activities.Add(new Activity { Type = "Activities", Value = "CanRead" });
                activities.Add(new Activity { Type = "Activities", Value = "CanUpdate" });
                activities.Add(new Activity { Type = "Activities", Value = "CanDelete" });
                activities.Add(new Activity { Type = "Video", Value = "CanCreate" });
                activities.Add(new Activity { Type = "Video", Value = "CanRead" });
                activities.Add(new Activity { Type = "Video", Value = "CanUpdate" });
                activities.Add(new Activity { Type = "Video", Value = "CanDelete" });
                activities.Add(new Activity { Type = "Video", Value = "CanSend" });                
                activities.Add(new Activity { Type = "Approval", Value = "CanApproveVideo" });

                activities.ForEach(x => 
                {
                    unitOfWork.Activities.Add(x);
                });
                unitOfWork.SaveChanges();
            }            
            
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
                    if (result.Succeeded)
                    {
                        if (activities != null)
                        {
                            activities.ForEach(x => 
                            {
                                mgr.AddClaim(u.Id, new Claim(x.Type, x.Value));
                            });
                        }
                    }
                }                
            }
        }
    }
}