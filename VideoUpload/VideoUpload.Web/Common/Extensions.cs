using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                return dt.ToString();
            }
            //if there is no offset in session return the datetime in server timezone
            return dt.ToLocalTime().ToString();
        }
    }
}