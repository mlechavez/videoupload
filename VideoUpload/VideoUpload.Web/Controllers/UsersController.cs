using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VideoUpload.Web.Models.UserViewModels;
using VideoUpload.Web.Models.Identity;
using VideoUpload.Core;
using System.Security.Claims;
using VideoUpload.Web.Common;
using VideoUpload.Web.Models;

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
        [AccessActionFilter(Type = "ManageUser", Value = "CanRead")]
        public async Task<ActionResult> List(int? page)
        {
            var users = await _uow.Users.GetAllAsync();

            var viewModel = new List<UserViewModel>();

            users.ForEach(x => 
            {
                viewModel.Add(new UserViewModel
                {
                    UserID = x.UserID,
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    JobTitle = x.JobTitle,
                    EmployeeNo = x.EmployeeNo,
                    Email = x.Email,
                    EmailPass = x.EmailPass
                });       
            });

            return View(viewModel.ToPagedList(page ?? 1, 15));
        }

        [Route("new")]
        [AccessActionFilter(Type = "ManageUser", Value = "CanCreate")]
        public ActionResult New()
        {
            ViewBag.BranchID = new SelectList(_uow.Branches.GetAll(), "BranchID", "BranchName");
            return View();
        }

        [HttpPost]
        [Route("new")]
        [AccessActionFilter(Type = "ManageUser", Value = "CanCreate")]
        public async Task<ActionResult> New(CreateUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var identityUser = new IdentityUser
                {
                    UserName = viewModel.UserName.Trim(),                    
                    FirstName = viewModel.FirstName.Trim(),
                    LastName = viewModel.LastName.Trim(),
                    JobTitle = viewModel.JobTitle.Trim(),
                    EmployeeNo = viewModel.EmployeeNo.Trim(),
                    Email = viewModel.Email.Trim(),
                    EmailPass = viewModel.EmailPass.Trim(),
                    IsActive = true,
                    PhoneNumber = viewModel.PhoneNumber.Trim(),
                    DirectLine = viewModel.DirectLine.Trim(),
                    FaxNumber = viewModel.FaxNumber.Trim(),
                    MobileNumber = viewModel.MobileNumber.Trim(),
                    WorkAddress = viewModel.WorkAddress.Trim(),
                    BranchID = viewModel.BranchID
                };
                var result = await _mgr.CreateAsync(identityUser, viewModel.Password);                
               
                if (result.Succeeded)
                {
                    result = await _mgr.SetEmailAsync(identityUser.Id, viewModel.Email);

                    if (result.Succeeded)
                    {
                        var defaultActivities = _uow.Activities.GetAllByType("Video");
                        defaultActivities.ForEach(activity => 
                        {
                            //exclude the delete access
                            if (activity.Value != "CanDelete")
                            {
                                result = _mgr.AddClaim(identityUser.Id, new Claim(activity.Type, activity.Value));
                                if (!result.Succeeded)
                                {
                                    AddErrors(result);
                                    return;
                                }                                
                            }                            
                        });
                        return RedirectToAction("list");
                    }
                    AddErrors(result);
                }
                AddErrors(result);
            }
            ViewBag.BranchID = new SelectList(_uow.Branches.GetAll(), "BranchID", "BranchName");
            return View(viewModel);
        }

        [Route("{userName}/edit")]
        [AccessActionFilter(Type = "ManageUser", Value = "CanUpdate")]
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
                IsActive = user.IsActive,
                PhoneNumber = user.PhoneNumber,
                DirectLine = user.DirectLine,
                FaxNumber = user.FaxNumber,
                MobileNumber = user.MobileNumber,
                WorkAddress = user.WorkAddress
                
            };
            ViewBag.BranchID = new SelectList(_uow.Branches.GetAll(), "BranchID", "BranchName", user.BranchID);
            return View(viewModel);
        }

        [HttpPost]
        [Route("{userName}/edit")]
        [AccessActionFilter(Type = "ManageUser", Value = "CanUpdate")]
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
                user.IsActive = viewModel.IsActive;
                user.PhoneNumber = viewModel.PhoneNumber;
                user.DirectLine = viewModel.DirectLine;
                user.FaxNumber = viewModel.FaxNumber;
                user.MobileNumber = viewModel.MobileNumber;
                user.WorkAddress = viewModel.WorkAddress;
                user.BranchID = viewModel.BranchID;
                
                var result = await _mgr.UpdateAsync(user);

                if (result.Succeeded)
                {
                    result = await _mgr.SetEmailAsync(user.Id, user.Email);
                    if (result.Succeeded) return RedirectToAction("list");
                }                
                AddErrors(result);

            }
            ViewBag.BranchID = new SelectList(_uow.Branches.GetAll(), "BranchID", "BranchName", viewModel.BranchID);
            return View(viewModel);
        }

        [ActionName("manage-claims")]
        [Route("{userID}/{userName}/manage-claims")]
        [AccessActionFilter(Type = "ManageUser", Value = "CanManageClaims")]
        public async Task<ActionResult> ManageClaims(string userID, string userName)
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
        [ActionName("manage-claims")]
        [Route("{userID}/{userName}/manage-claims")]
        [HttpPost]
        [AccessActionFilter(Type = "ManageUser", Value = "CanManageClaims")]
        public async Task<ActionResult> ManageClaims(FormCollection formCollection)
        {
            var userID = formCollection["UserID"];
            var frmManageUser = formCollection["ManageUser"];
            var frmVideo = formCollection["Video"];
            var frmApproval = formCollection["Approval"];

            var userClaims = await _mgr.GetClaimsAsync(userID);

            var manageUserClaims = userClaims.Where(x => x.Type == "ManageUser").ToList();
            var videoUserClaims = userClaims.Where(x => x.Type == "Video").ToList();
            var approvalUserClaims = userClaims.Where(x => x.Type == "Approval").ToList();

            AddRemoveClaims(userID, manageUserClaims, frmManageUser, "ManageUser");
            AddRemoveClaims(userID, videoUserClaims, frmVideo, "Video");
            AddRemoveClaims(userID, approvalUserClaims, frmApproval, "Approval");

            //due to an updateAsync inside the RemoveClaimsAsync.. I have to delete manually the null userid userClaims
            RemoveUserClaimsManually();
            return RedirectToAction("list");
        }

        [Route("{userName}/change-password")]
        public async Task<ActionResult> ChangePassword(string userName)
        {
            var user = await _mgr.FindByNameAsync(userName);

            if (user == null)
            {
                return View("_ResourceNotFound");
            }

            var viewModel = new ChangePasswordViewModel
            {
                UserID = user.Id,
                UserName = user.UserName
            };

            ViewBag.Header = $"Change {user.UserName}'s password";
            return View("Change-Password", viewModel);
        }
        
        [HttpPost]
        [Route("{userName}/change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _mgr.ChangePasswordAsync(viewModel.UserID, viewModel.OldPassword, viewModel.NewPassword);

                if (result.Succeeded)
                {
                    return RedirectToAction("edit", new { userName = viewModel.UserName });
                }
                AddErrors(result);
            }
            
            return View("Change-Password", viewModel);
        }

        [Route("{userName}/password-reset")]
        public async Task<ActionResult> PasswordReset(string userName)
        {
            var user = await _mgr.FindByNameAsync(userName);
            var viewModel = new PasswordResetViewModel { Email = user.Email };
            ViewBag.Header = $"Reset the password of {userName}";
            ViewBag.EmailLabel = $"Please verify {user.UserName }'s email address";
            return View("Password-Reset", viewModel);
        }
        [HttpPost]
        [Route("{userName}/password-reset")]
        public async Task<ActionResult> PasswordReset(PasswordResetViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.Email))
            {
                ViewBag.Message = "Email is required";
            }
            var user = await _mgr.FindByEmailAsync(viewModel.Email);

            if (user == null)
            {
                ViewBag.Message = "We cannot find your email to our records. Please try again";
                return View("Password-Reset");
            }

            var key = await _mgr.GeneratePasswordResetTokenAsync(user.Id);

            var url = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("ConfirmPasswordReset", "Account", new { key = key, id = user.Id });

            await _mgr.CustomSendEmailAsync(user.Id, "Reset Password Request", "Click here to reset your password: " + url, user.Email);

            ViewBag.Message = $"{user.FirstName } will receive an email notification for reset password";
            return View("_PasswordResetRequestSentSuccess");
        }

        [Route("{userName}/change-email-password")]
        public async Task<ActionResult> ChangeEmailPassword(string userName)
        {
            var user = await _mgr.FindByNameAsync(userName);
            var viewModel = new EmailPasswordViewModel { UserID = user.Id, EmailPassword = user.EmailPass };

            ViewBag.Header = $"Change email password for { user.FirstName }";
            return View(viewModel);
        }

        [Route("{userName}/change-email-password")]
        [HttpPost]
        public async Task<ActionResult> ChangeEmailPassword(EmailPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _mgr.FindByIdAsync(viewModel.UserID);

                if (user == null) return View("ResourceNotFound");

                user.EmailPass = viewModel.EmailPassword;

                var result = await _mgr.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("edit", new { userName = user.UserName });
                }
                else
                {
                    AddErrors(result);
                }                
            }
            ModelState.AddModelError("", "Email password is required");               
            return View(viewModel);
        }
        private void AddRemoveClaims(string userID, List<Claim> userClaims, string selectedClaims, string type)
        {
            if (selectedClaims == null)
            {
                if (userClaims.Count > 0)
                {
                    userClaims.ForEach(x =>
                    {
                        _mgr.RemoveClaim(userID, x);
                       
                    });
                }
            }
            else
            {
                var arraySelectedClaims = selectedClaims.Split(',');

                if (userClaims.Count == 0)
                {
                    Array.ForEach(arraySelectedClaims, x =>
                    {
                        _mgr.AddClaim(userID, new Claim(type, x));
                    });
                }
                else
                {
                    if (userClaims.Count > arraySelectedClaims.Count())
                    {
                        userClaims.ForEach(x => 
                        {
                            if (!arraySelectedClaims.Contains(x.Value))
                            {
                                _mgr.RemoveClaim(userID, x);
                            }
                        });
                    }
                    else if (userClaims.Count < arraySelectedClaims.Count())
                    {
                        Array.ForEach(arraySelectedClaims, x =>
                        {
                            if (!userClaims.Exists(y => y.Value == x))
                            {
                                //var currentClaim = userClaims.FirstOrDefault(c => c.Value == x);
                                _mgr.AddClaim(userID, new Claim(type, x));                                
                            }
                        });
                    }
                    else
                    {
                        var existingClaims = new List<string>();
                        Array.ForEach(arraySelectedClaims, x =>
                        {
                            if (!userClaims.Exists(y => y.Value == x))
                            {
                                _mgr.AddClaim(userID, new Claim(type, x));
                            }
                            existingClaims.Add(x);
                        });
                        Array.ForEach(userClaims.ToArray(), r =>
                        {
                            if (!existingClaims.Exists(q => q == r.Value))
                            {
                                _mgr.RemoveClaim(userID, r);                               
                            }
                        });
                    }
                }
            }
        }

        private void RemoveUserClaimsManually()
        {
            var userClaims = _uow.UserClaims.GetAllNullUserID();
            if (userClaims.Count > 0)
            {
                _uow.UserClaims.RemoveRange(userClaims);
                _uow.SaveChanges();
            }            
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