using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VideoUpload.Core;
using VideoUpload.Core.Entities;
using VideoUpload.Web.Models.ActivityViewModels;

namespace VideoUpload.Web.Controllers
{
    [RoutePrefix("activities")]
    public class ActivitiesController : Controller
    {
        private readonly IUnitOfWork _uow;

        public ActivitiesController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        
        [Route]
        public async Task<ActionResult> List()
        {
            var activities = await _uow.Activities.GetAllAsync();
            var viewModel = new List<ActivityViewModel>();
            activities.ForEach(x => 
            {
                viewModel.Add(new ActivityViewModel
                {
                    ActivityID = x.ActivityID,
                    Type = x.Type,
                    Value = x.Value,
                    Description = x.Description
                });
            });
            return View(viewModel);
        }

        [Route("new")]
        public ActionResult New()
        {            
            return View();
        }

        [Route("new")]
        [HttpPost]
        public ActionResult New(CreateActivityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //_uow.Activities
            }
            return View(viewModel);
        }
    }
}