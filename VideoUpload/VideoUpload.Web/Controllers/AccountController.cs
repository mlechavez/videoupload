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
                        var ci = await _mgr.CreateIdentityAsync(user, "Cookie");
                        var ctx = Request.GetOwinContext();
                        var authMgr = ctx.Authentication;
                        authMgr.SignIn(ci);
                        return RedirectToAction("index", "videos");
                    }
                }
            }
            return View(viewModel);
        }

        public ActionResult Signout()
        {
            var ctx = Request.GetOwinContext();
            var authMgr = ctx.Authentication;
            authMgr.SignOut("Cookie");

            return RedirectToAction("index", "videos");
        }        
    }
}