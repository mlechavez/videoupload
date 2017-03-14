using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace VideoUpload.Web.Common
{
    public class AccessActionFilter : ActionFilterAttribute
    {
        public string Type { get; set; }
        public string Value { get; set; }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var identity = filterContext.HttpContext.User as ClaimsPrincipal;
            var activities = identity.FindAll(Type).ToList();

            if (activities.Count == 0)
            {
                filterContext.Result = new ViewResult { ViewName = "_UnAuthorized" };
            }
            var activity = activities.FirstOrDefault(x => x.Value == Value);

            if (activity == null)
            {
                filterContext.Result = new ViewResult { ViewName = "_UnAuthorized" };
            }
        }
    }
}