using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoUpload.Web.Models.Identity
{
    public class IdentityUser : IUser<string>
    {
        public IdentityUser()
        {
            Id = Guid.NewGuid().ToString();
        }
        public IdentityUser(string username)
            :this()
        {
            UserName = username;
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string EmployeeNo { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsActive { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string EmailPass { get; set; }
    }
}