using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
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
                new {                    
                    year = post.DateUploaded.Year,
                    month = post.DateUploaded.Month,
                    postID = post.PostID,
                    plateNo = post.PlateNumber },
                new { title = post.PlateNumber });
        }        
    }
}