using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoUpload.Core.Entities
{
    public class Branch
    {
        private ICollection<User> _users;
        private ICollection<Post> _posts;
        private ICollection<Jobcard> _jobcards;

        public int BranchID { get; set; }        
        public string BranchName { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }

        public virtual ICollection<User> Users
        {
            get { return _users ?? (_users = new List<User>()); }
            set { _users = value; }
        }

        public virtual ICollection<Post> Posts
        {
            get { return _posts ?? (_posts = new List<Post>()); }
            set { _posts = value; }
        }

        public virtual ICollection<Jobcard> Jobcards
        {
            get { return _jobcards ?? (_jobcards = new List<Jobcard>()); }
            set { _jobcards = value; }
        }
    }
}
