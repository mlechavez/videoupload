using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace VideoUpload.Web.Common
{
    public class AppUser : ClaimsPrincipal
    {
        public AppUser(ClaimsPrincipal principal)
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

        public List<Claim> ManageUser
        {
            get { return FindAll("ManageUser").ToList(); }
        }

        public List<Claim> Approval
        {
            get { return FindAll("Approval").ToList(); }
        }

    }
}