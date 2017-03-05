using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoUpload.Core.Entities
{
    public class UserClaim
    {
        public int ClaimID { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string UserID { get; set; }

        public virtual User User { get; set; }
    }
}
