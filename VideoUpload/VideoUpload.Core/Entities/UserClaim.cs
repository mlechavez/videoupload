using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoUpload.Core.Entities
{
    public class UserClaim
    {
        #region Private Fields
        private User _user;
        #endregion

        public int ClaimID { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string UserID { get; set; }

        public virtual User User
        {
            get { return _user;  }
            set
            {
                //if (value == null) throw new ArgumentNullException("value");                
                _user = value;
                //UserID = value.UserID;
            }
        }
    }
}
