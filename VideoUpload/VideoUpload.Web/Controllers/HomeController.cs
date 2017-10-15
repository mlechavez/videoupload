using System.Web.Mvc;
using VideoUpload.Core;
using VideoUpload.Web.Models.Home;

namespace VideoUpload.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _uow;
        public HomeController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Route("about")]
        public ActionResult About()
        {
            return View();
        }

        [Route("contact-us")]
        public ActionResult Contact(string branchName)
        {
            ViewBag.Header = "Contact us";

            if (!string.IsNullOrWhiteSpace(branchName))
            {
                var branch = _uow.Branches.GetByName(branchName);
                return View(branch);
            }

            ViewBag.Header = "Contact us";

            return View();
        }

        public PartialViewResult GetBranches(string videoUrl)
        {
            ViewBag.BranchList = ContactWidgetViewModel.Create(_uow, 1, 100);
            return PartialView("_ContactUsWidget");
        }
    }
}