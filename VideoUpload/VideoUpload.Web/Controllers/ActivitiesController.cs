using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoUpload.Core;

namespace VideoUpload.Web.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly IUnitOfWork _uow;

        public ActivitiesController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        // GET: Activities
        public ActionResult Index()
        {
            var activities = _uow.Activities.GetAll();
            return View();
        }
    }
}