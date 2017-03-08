using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace VideoUpload.Web.Common
{
    public abstract class AppViewPage<T> : WebViewPage<T>
    {
        protected AppUser CurrentUser
        {
            get { return new AppUser(User as ClaimsPrincipal); }
        }
    }

    public abstract class AppViewPage : AppViewPage<dynamic>
    {
    }
}