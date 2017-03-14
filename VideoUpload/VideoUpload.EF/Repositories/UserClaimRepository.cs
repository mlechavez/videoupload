using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;
using VideoUpload.Core.Repositories;

namespace VideoUpload.EF.Repositories
{
    internal class UserClaimRepository : Repository<UserClaim>, IUserClaimRepository
    {
        public UserClaimRepository(AppDbContext context) : base(context)
        {
        }

        public List<UserClaim> GetAllNullUserID()
        {
            return Set.Where(x => x.UserID == null).ToList(); 
        }
    }
}
