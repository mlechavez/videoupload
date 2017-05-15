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

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public string JobTitle
        {
            get { return FindFirst("jobtitle").Value; }
        }
        public string Email
        {
            get { return FindFirst("email").Value; }
        }
        public string EmailPass
        {
            get { return FindFirst("emailpass").Value; }
        }

        public string WorkAddress
        {
            get { return FindFirst("workaddress").Value; }
        }
        public string PhoneNumber
        {
            get { return FindFirst("phonenumber").Value; }
        }

        public string DirectLine
        {
            get { return FindFirst("directline").Value; }
        }

        public string FaxNumber
        {
            get { return FindFirst("faxnumber").Value; }
        }
        public string MobileNumber
        {
            get { return FindFirst("mobilenumber").Value; }
        }
        public List<Claim> ManageUser
        {
            get { return FindAll("ManageUser").ToList(); }
        }

        public List<Claim> Approval
        {
            get { return FindAll("Approval").ToList(); }
        }

        public int BranchID
        {
            get
            {
                var branchID = Convert.ToInt32(FindFirst("branchID").Value);
                return branchID;
            }
        }
    }
}