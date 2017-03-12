using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Models.UserViewModels
{
    public class UserActivityViewModel
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public List<Activity> Activities { get; set; }
        public List<Claim> UserClaims { get; set; }
    }
}