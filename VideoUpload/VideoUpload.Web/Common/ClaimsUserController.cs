using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace VideoUpload.Web.Common
{
    public class ClaimsUserController : Controller
    {
        public AppUserClaim AppUserClaim
        {
            get
            {
                return new AppUserClaim(this.User as ClaimsPrincipal);
            }
        }
    }
}