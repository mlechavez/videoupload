using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VideoUpload.Web.Models;
using VideoUpload.Web.Models.Identity;

namespace VideoUpload.Web.Controllers
{
    [RouteArea("manage")]
    [RoutePrefix("users")]
    public class UsersController : Controller
    {
        private readonly UserManager _mgr;
        public UsersController(UserManager mgr)
        {
            _mgr = mgr;
        }        

        [Route("list")]
        public ActionResult List(int? page)
        {
            var users = _mgr.Users.ToList();
            return View(users.ToPagedList(page ?? 1, 20));
        }

        [Route("new")]
        public ActionResult New()
        {
            return View();
        }

        [Route("new")]
        [HttpPost]
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
        
        public ActionResult PasswordReset()
        {
            //_mgr.GeneratePasswordResetToken()
            return View();
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