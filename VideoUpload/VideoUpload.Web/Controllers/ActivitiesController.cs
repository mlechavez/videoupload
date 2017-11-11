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
        public async Task<ActionResult> New(CreateActivityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //fetch if exists
                var activity = _uow.Activities.GetByTypeAndValue(viewModel.Type, viewModel.Value);

                if (activity == null)
                {
                    var newActivity = new Activity
                    {
                        Type = viewModel.Type,
                        Value = viewModel.Value,
                        Description = viewModel.Description
                    };
                    _uow.Activities.Add(newActivity);
                    await _uow.SaveChangesAsync();
                    return RedirectToAction("list");
                }
                ModelState.AddModelError("", $"Type: {viewModel.Type} and Value: {viewModel.Value} already exists.");
            }
            return View(viewModel);
        }

        [Route("{activityID}/{description}/edit")]
        public async Task<ActionResult> Edit(int activityID, string description)
        {
            var activity = await _uow.Activities.GetByIdAsync(activityID);

            if (activity == null)
            {
                return View("_ResourceNotFound");
            }

            var viewModel = new ActivityViewModel
            {
                ActivityID = activity.ActivityID,
                Type = activity.Type,
                Value = activity.Value,
                Description = activity.Description
            };
            return View(viewModel);
        }

        [Route("{activityID}/{description}/edit")]
        [HttpPost]
        public async Task<ActionResult> Edit(ActivityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var activity = await _uow.Activities.GetByIdAsync(viewModel.ActivityID);

                if (activity == null)
                {
                    return View("_ResourceNotFound");
                }
                activity.Description = viewModel.Description;
                _uow.Activities.Update(activity);
                await _uow.SaveChangesAsync();
                return RedirectToAction("list");
            }

            return View(viewModel);
        }
    }
}