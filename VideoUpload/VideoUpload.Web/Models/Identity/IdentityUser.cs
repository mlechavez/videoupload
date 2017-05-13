using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Models.Identity
{
    public class IdentityUser : IUser<string>
    {
        private ICollection<UserClaim> _userClaims;
        private ICollection<Post> _posts;
        private Branch _branch;
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
        public string MobileNumber { get; set; }
        public string EmailPass { get; set; }
        public int? BranchID { get; set; }

        public virtual ICollection<UserClaim> UserClaims
        {
            get { return _userClaims ?? (_userClaims = new List<UserClaim>()); }
            set { _userClaims = value; }
        }

        public virtual ICollection<Post> Posts
        {
            get { return _posts ?? (_posts = new List<Post>()); }
            set { _posts = value; }
        }
        public virtual Branch Branch
        {
            get { return _branch; }
            set { _branch = value; }
        }
    }
}