using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VideoUpload.Web.Models.UserViewModels;
using VideoUpload.Web.Models.Identity;
using VideoUpload.Core;
using System.Security.Claims;

namespace VideoUpload.Web.Controllers
{

    [RoutePrefix("users")]
    public class UsersController : Controller
    {
        private readonly UserManager _mgr;
        private readonly IUnitOfWork _uow;

        public UsersController(UserManager mgr, IUnitOfWork uow)
        {
            _mgr = mgr;
            _uow = uow;
        }

        [Route]
        public ActionResult List(int? page)
        {
            var users = _mgr.Users.ToList();

            var viewModel = new List<UserViewModel>();

            users.ForEach(x => 
            {
                viewModel.Add(new UserViewModel
                {
                    UserID = x.Id,
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    JobTitle = x.JobTitle,
                    EmployeeNo = x.EmployeeNo,
                    Email = x.Email,
                    EmailPass = x.EmailPass
                });       
            });

            return View(viewModel.ToPagedList(page ?? 1, 20));
        }

        [Route("new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Route("new")]                
        public async Task<ActionResult> New(CreateUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var identityUser = new IdentityUser
                {
                    UserName = viewModel.UserName,                    
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    JobTitle = viewModel.Designation,
                    EmployeeNo = viewModel.EmployeeNo,
                    Email = viewModel.Email,
                    EmailPass = viewModel.EmailPass,
                    IsActive = true                    
                };
                var result = await _mgr.CreateAsync(identityUser, viewModel.Password);                
               
                if (result.Succeeded)
                {
                    result = await _mgr.SetEmailAsync(identityUser.Id, viewModel.Email);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("list");
                    }
                    AddErrors(result);
                }
                AddErrors(result);
            }
            return View(viewModel);
        }

        [Route("{userName}/edit")]
        public async Task<ActionResult> Edit(string userName)
        {          
            if (string.IsNullOrWhiteSpace(userName)) return View("_ResourceNotFound");

            var user = await _mgr.FindByNameAsync(userName);

            if (user == null) return View("_ResourceNotFound");

            var viewModel = new UserViewModel
            {
                UserID = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                JobTitle = user.JobTitle,
                EmployeeNo = user.EmployeeNo,
                Email = user.Email,
                EmailPass = user.EmailPass,
                IsActive = user.IsActive
            };
            return View(viewModel);
        }

        [HttpPost]
        [Route("{userName}/edit")]
        public async Task<ActionResult> Edit(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _mgr.FindById(viewModel.UserID);

                if (user == null) return View("_ResourceNotFound");

                user.UserName = viewModel.UserName;
                user.FirstName = viewModel.FirstName;
                user.LastName = viewModel.LastName;
                user.JobTitle = viewModel.JobTitle;
                user.EmployeeNo = viewModel.EmployeeNo;
                user.Email = viewModel.Email;
                user.EmailPass = viewModel.EmailPass;
                user.IsActive = viewModel.IsActive;
                
                var result = await _mgr.UpdateAsync(user);
                if (result.Succeeded)
                {
                    result = await _mgr.SetEmailAsync(user.Id, user.Email);
                    if (result.Succeeded) return RedirectToAction("list");
                }                
                AddErrors(result);
            }
            return View(viewModel);
        }

        [ActionName("add-claims")]
        [Route("{userID}/{userName}/add-claims")]        
        public async Task<ActionResult> AddClaims(string userID, string userName)
        {
            var activities = await _uow.Activities.GetAllAsync();
            var userClaims = await _mgr.GetClaimsAsync(userID);
            var viewModel = new UserActivityViewModel
            {
                UserName = userName,
                Activities = activities,
                UserClaims = userClaims.ToList()
            };
            return View(viewModel);
        }
        [ActionName("add-claims")]
        [Route("{userID}/{userName}/add-claims")]
        [HttpPost]
        public async Task<ActionResult> AddClaims(FormCollection formColletion)
        {
            var fcUserID = formColletion["UserID"];
            var fcManageUser = formColletion["ManageUser"];
            var fcVideoSection = formColletion["Video"];
            var fcApproval = formColletion["Approval"];

            var userClaims = await _mgr.GetClaimsAsync(fcUserID);
            var manageUserClaims = userClaims.Where(x => x.Type == "ManageUser").ToList();
            if (fcManageUser != null)
            {
                //make it as array of string
                string[] manageUser  = fcManageUser.Split(',');
                //filter the claims to ManageUser
                               
                    foreach (var selected in manageUser)
                    {
                        if (manageUserClaims.Any(x=>x.Value == selected))
                        {
                            //var currentClaim = manageUserClaims.
                            //await _mgr.RemoveClaimAsync(fcUserID)
                        }
                    }                                                                                 
            }
            else
            {
                foreach (var claim in manageUserClaims)
                {
                    await _mgr.RemoveClaimAsync(fcUserID, claim);
                }
            }

            return RedirectToAction("list");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}