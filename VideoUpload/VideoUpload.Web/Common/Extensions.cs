using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Common
{
    public static class Extensions
    {
        public static string ToClientTime(this DateTime dt)
        {
            var timeoffSet = HttpContext.Current.Session["timezoneoffset"];

            if (timeoffSet != null)
            {
                var offset = int.Parse(timeoffSet.ToString());
                dt = dt.AddMinutes(-1 * offset);
                return dt.ToString("MMM dd, yyyy hh:mm:ss");
            }
            //if there is no offset in session return the datetime in server timezone
            return dt.ToLocalTime().ToString("MMM dd, yyyy hh:mm:ss");
        }

        public static string HyphenUrl(this HtmlHelper htmlHelper, string actionName)
        {
            var cleanActionName = actionName.ToLower().Replace(" ", "-");
            cleanActionName = Regex.Replace(cleanActionName, @"[^a-zA-Z0-9\/+ -]", "");
            return cleanActionName;
        }
        public static string SpaceUrl(this HtmlHelper htmlHelper, string actionName)
        {
            var cleanActionName = actionName.ToLower().Replace("-", " ");
            cleanActionName = Regex.Replace(cleanActionName, @"[^a-zA-Z0-9\/+ -]", "");
            return cleanActionName;
        }

        public static MvcHtmlString PostActionLink(this HtmlHelper htmlHelper, Post post)
        {
            return htmlHelper.ActionLink(post.PlateNumber, "post", "videos",
                new
                {
                    year = post.DateUploaded.Year,
                    month = post.DateUploaded.Month,
                    postID = post.PostID,
                    plateNo = post.PlateNumber
                },
                new { title = post.PlateNumber });
        }

        public static string IsActive(this HtmlHelper htmlHelper, string action, string controller)
        {
            var viewContext = htmlHelper.ViewContext;
            bool isChildAction = viewContext.IsChildAction;

            if (isChildAction)
                viewContext = viewContext.ParentActionViewContext;

            RouteValueDictionary routes = viewContext.RouteData.Values;
            string currentAction = routes["action"].ToString().ToLower();
            string currentController = routes["controller"].ToString().ToLower();

            if (string.IsNullOrEmpty(action))
                action = currentAction;
            if (string.IsNullOrEmpty(controller))
                controller = currentController;

            string[] acceptedAction = action.Trim().Split(',').Distinct().ToArray();
            string[] acceptedController = controller.Trim().Split(',').Distinct().ToArray();

            return acceptedAction.Contains(currentAction) && acceptedController.Contains(currentController) ? "active" : string.Empty;
        }        
    }
}