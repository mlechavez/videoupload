using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace VideoUpload.Web.Common
{
    public class AppUserClaim : ClaimsPrincipal
    {
        public AppUserClaim(ClaimsPrincipal principal)
            :base(principal)
        {
        }

        public string FirstName
        {
            get { return FindFirst("firstname").Value; }
        }
        public string LastName
        {
            get { return FindFirst("lastname").Value; }
        }
        public string Email
        {
            get { return FindFirst("email").Value; }
        }
        public string EmailPass
        {
            get { return FindFirst("emailpass").Value; }
        }

        //public List<Claim> Access
        //{
        //    get { return FindAll("approval").ToList(); }            
        //}


    }
}