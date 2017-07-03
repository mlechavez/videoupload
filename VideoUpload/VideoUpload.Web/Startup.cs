using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using VideoUpload.Web.Models.Identity;
using VideoUpload.Core.Entities;
using VideoUpload.EF;
using System.Security.Claims;
using VideoUpload.Core;

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
            var unitOfWork = DependencyResolver.Current.GetService(typeof(IUnitOfWork)) as UnitOfWork;

            var activities = unitOfWork.Activities.GetAll();
            var branches = unitOfWork.Branches.GetAll();

            if (activities.Count == 0)
            {                
                activities.Add(new Activity { Type = "ManageUser", Value = "CanCreate", Description = "Can create a user." });
                activities.Add(new Activity { Type = "ManageUser", Value = "CanRead", Description = "Can view user list" });
                activities.Add(new Activity { Type = "ManageUser", Value = "CanUpdate", Description = "Can update a user" });
                activities.Add(new Activity { Type = "ManageUser", Value = "CanDelete", Description = "Can delete a user" });
                activities.Add(new Activity { Type = "ManageUser", Value = "CanManageClaims", Description = "Can manage the access of users"});
                activities.Add(new Activity { Type = "Activities", Value = "CanCreate", Description = "Can create activity" });
                activities.Add(new Activity { Type = "Activities", Value = "CanRead", Description = "Can view activity list" });
                activities.Add(new Activity { Type = "Activities", Value = "CanUpdate", Description = "Can update activity" });
                activities.Add(new Activity { Type = "Activities", Value = "CanDelete", Description = "Can delete activity" });
                activities.Add(new Activity { Type = "Video", Value = "CanCreate", Description = "Can create a post and upload a video" });
                activities.Add(new Activity { Type = "Video", Value = "CanRead", Description = "Can view list of posts and its details" });
                activities.Add(new Activity { Type = "Video", Value = "CanUpdate", Description = "Can update a post" });
                activities.Add(new Activity { Type = "Video", Value = "CanDelete", Description = "Can delete a post" });
                activities.Add(new Activity { Type = "Video", Value = "CanSend", Description = "Can send email or sms" });                
                activities.Add(new Activity { Type = "Approval", Value = "CanApproveVideo", Description = "Can approve video to be sent to customer" });

                activities.ForEach(x => 
                {
                    unitOfWork.Activities.Add(x);
                });                
            }

            if (branches.Count == 0)
            {
                branches.Add(new Branch
                {
                    BranchName = "PCD",
                    Name = "Showroom",
                    Address = "P.O Box 23619 - Airport St. Doha, Qatar",
                    PhoneNumber = "+974 44599 666",
                    FaxNumber = "+974 44666 993",
                    Email = "info@boraq-porsche.com.qa"
                });
                branches.Add(new Branch
                {
                    BranchName = "ST27",
                    Name =  "Porsche Service Centre St. 27",
                    Address = "27th Street Industrial Area",
                    PhoneNumber = "+974 44599 777",
                    FaxNumber = "+974 44503 366",
                    Email = "info@boraq-porsche.com.qa"
                });
                branches.Add(new Branch
                {
                    BranchName = "ST16",
                    Name = "Porsche Service Centre St. 16",
                    Address = "16th Street Industrial Area",
                    PhoneNumber = "+974 44599 799",
                    FaxNumber = "+974 44111 027",
                    Email = "info@boraq-porsche.com.qa"                    
                });
                branches.Add(new Branch
                {
                    BranchName = "QSC",
                    Name = "Porsche Quick Service Centre",
                    Address = "City Petol Station, Al Gharaffa",
                    PhoneNumber = "+974 44599 733",
                    FaxNumber = "+974 44863 388",
                    Email = "info@boraq-porsche.com.qa"
                });                

                branches.ForEach(x => { unitOfWork.Branches.Add(x); });
            }

            unitOfWork.SaveChanges();


            var user = mgr.FindByName("admin");

            if (user == null)
            {
                var u = new IdentityUser
                {
                    UserName = "admin",

                    FirstName = "Systems",
                    LastName = "Admin",
                    JobTitle = "Web App Administrator",
                    EmployeeNo = "000",
                    IsActive = true,
                    Email = "remoteyourporsche@boraq-porsche.com.qa",
                    EmailConfirmed = true,
                    EmailPass = "Boraq@23619",
                    BranchID = 2,
                    WorkAddress = "16th Street Industrial Area",
                    PhoneNumber  = "+974 44599800",
                    DirectLine = "+974 44599800 ext. 170",
                    FaxNumber = "+974 44111027",
                    MobileNumber = "+974 700 64955"

                };
                var result = mgr.Create(u, "Alboraq.@pp");

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