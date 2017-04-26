using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VideoUpload.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        [Route("contact-us")]
        public ActionResult Contact()
        {
            ViewBag.Header = "Contact us";

            return View();
        }
    }
}