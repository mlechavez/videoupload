﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoUpload.Core.Entities
{
    public class User
    {
        private ICollection<UserClaim> _userClaims;
        private ICollection<Post> _posts;
        private ICollection<History> _histories;
        private Branch _branch;

        public string UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string EmployeeNo { get; set; }
        public string Email { get; set; }
        public string WorkAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string DirectLine { get; set; }
        public string FaxNumber { get; set; }        
        public bool EmailConfirmed { get; set; }
        public bool IsActive { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string EmailPass { get; set; }
        public string MobileNumber { get; set; }
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

        public virtual ICollection<History> Histories
        {
            get { return _histories ?? (_histories = new List<History>()); }
            set { _histories = value; }
        }
    }
}
