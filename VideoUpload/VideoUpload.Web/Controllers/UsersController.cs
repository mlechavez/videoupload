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

namespace VideoUpload.Web.Controllers
{

    [RoutePrefix("users")]
    public class UsersController : Controller
    {
        private readonly UserManager _mgr;
        public UsersController(UserManager mgr)
        {
            _mgr = mgr;
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

        [Route("{userID}/{userName}/edit")]
        public async Task<ActionResult> Edit(string userID)
        {          
            if (string.IsNullOrWhiteSpace(userID)) return View("_ResourceNotFound");

            var user = await _mgr.FindByIdAsync(userID);

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
        [Route("{userID}/{userName}/edit")]
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
               
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}