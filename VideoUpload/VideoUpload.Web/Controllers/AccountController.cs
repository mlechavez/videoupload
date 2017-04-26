using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VideoUpload.Web.Models;
using VideoUpload.Web.Models.Identity;

namespace VideoUpload.Web.Controllers
{
    [AllowAnonymous]    
    public class AccountController : Controller
    {
        private readonly UserManager _mgr;

        public AccountController(UserManager mgr)
        {
            _mgr = mgr;
        }
        
        public ActionResult Signin(string ReturnUrl)
        {
            var viewModel = new LoginViewModel { ReturnUrl = ReturnUrl };
            return View(viewModel);
        }
        // GET: Account
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

        public ActionResult PasswordReset()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PasswordReset(string email)
        {            
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Message = "Email is required";
            }
            var user = await _mgr.FindByEmailAsync(email);

            if (user == null)
            {
                ViewBag.Message = "We cannot find your email to our records. Please try again";
                return View();
            }

            var key = await _mgr.GeneratePasswordResetTokenAsync(user.Id);

            var url = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("ConfirmPasswordReset", "Account", new { key = key, id = user.Id });

            await _mgr.CustomSendEmailAsync(user.Id, "Reset Password Request", "Click here to reset your password: " + url, user.Email, user.EmailPass);

            return View("_PasswordResetRequestSentSuccess");
        }

        public ActionResult ConfirmPasswordReset(string key, string id)
        {
            var viewModel = new ConfirmPasswordViewModel
            {
                Key = key,
                Id = id
            };
            return View(viewModel);
        }

        [HttpPost]
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
            return View(viewModel);
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