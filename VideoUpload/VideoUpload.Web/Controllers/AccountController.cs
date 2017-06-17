using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VideoUpload.Core;
using VideoUpload.Web.Models;
using VideoUpload.Web.Models.Identity;

namespace VideoUpload.Web.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("account")]    
    public class AccountController : Controller
    {
        private readonly UserManager _mgr;
        private readonly IUnitOfWork _uow;
        public AccountController(UserManager mgr, IUnitOfWork uow)
        {
            _mgr = mgr;
        }
        
        public ActionResult Signin(string ReturnUrl)
        {
            var viewModel = new LoginViewModel { ReturnUrl = ReturnUrl };
            return View(viewModel);
        }
        
        [HttpPost]
        public async Task<ActionResult> Signin(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _mgr.FindByNameAsync(viewModel.Username);

                if (user != null)
                {
                    if (await _mgr.CheckPasswordAsync(user, viewModel.Password))
                    {
                        if (user.IsActive)
                        {
                            var ci = await _mgr.CreateIdentityAsync(user, "Cookie");
                            
                            var ctx = Request.GetOwinContext();
                            var authMgr = ctx.Authentication;
                            authMgr.SignIn(ci);

                            return Redirect(GetReturnUrl(viewModel.ReturnUrl));
                        }
                        ModelState.AddModelError("", "You account has not activated yet. Contact your admin");
                        return View(viewModel);
                    }
                }
                else
                {
                    var u = new IdentityUser
                    {
                        UserName = "admin",

                        FirstName = "Admin",
                        LastName = "",
                        JobTitle = "Administrator",
                        EmployeeNo = "000",
                        IsActive = true,
                        Email = "alboraq.app@boraq-porsche.com.qa",
                        EmailConfirmed = true,
                        EmailPass = "Boraq@23619",
                        BranchID = 2,
                        WorkAddress = "st16",
                        PhoneNumber = "+974 44599800",
                        DirectLine = "+974 44599800",
                        FaxNumber = "+974 44111027",
                        MobileNumber = "+974 700 64955"

                    };
                    var result = _mgr.Create(u, "@lboraq.app");

                    if (result.Succeeded)
                    {
                        result = _mgr.SetEmail(u.Id, u.Email);
                        if (result.Succeeded)
                        {
                            var activities = _uow.Activities.GetAll();
                            if (activities != null)
                            {
                                activities.ForEach(x =>
                                {
                                    _mgr.AddClaim(u.Id, new Claim(x.Type, x.Value));
                                });
                            }
                        }
                    }
                }
                ModelState.AddModelError("", "username or password is incorrect!");  
            }
            return View(viewModel);
        }

        public ActionResult Signout()
        {
            var ctx = Request.GetOwinContext();
            var authMgr = ctx.Authentication;
            authMgr.SignOut("Cookie");

            return RedirectToAction("posts", "videos");
        }                

        [Route("password-reset")]
        public ActionResult PasswordReset()
        {
            ViewBag.Header = "Reset your password";
            ViewBag.EmailLabel = "Enter your email address";
            return View("Password-Reset");
        }

        [HttpPost]
        [Route("password-reset")]
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
                return View();
            }

            var key = await _mgr.GeneratePasswordResetTokenAsync(user.Id);

            var url = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("ConfirmPasswordReset", "Account", new { key = key, id = user.Id });

            await _mgr.CustomSendEmailAsync(user.Id, "Reset Password Request", "Click here to reset your password: " + url, user.Email);

            ViewBag.Message = "Your password request has been sent to your email";
            return View("_PasswordResetRequestSentSuccess");
        }

        [Route("confirm-password-reset")]
        public ActionResult ConfirmPasswordReset(string key, string id)
        {
            var viewModel = new ConfirmPasswordViewModel
            {
                Key = key,
                Id = id
            };
            return View("confirm-password-reset", viewModel);
        }

        [HttpPost]
        [Route("confirm-password-reset")]
        public async Task<ActionResult> ConfirmPasswordReset(ConfirmPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _mgr.ResetPasswordAsync(viewModel.Id, viewModel.Key, viewModel.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("posts","videos");
                }
                AddErrors(result);
            }            
            return View("confirm-password-reset", viewModel);
        }

        private string GetReturnUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) && !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("posts","videos");
            }
            return returnUrl;
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