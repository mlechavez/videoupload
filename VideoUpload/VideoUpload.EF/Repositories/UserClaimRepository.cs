using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
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

        public List<UserClaim> GetAllByClaimTypeAndValue(string claimType, string claimValue)
        {
            return Set
                    .Include(u=>u.User)
                    .Where(type => type.ClaimType.Equals(claimType))
                    .ToList();
        }

        public Task<List<UserClaim>> GetAllByClaimTypeAndValueAsync(string claimType, string claimValue)
        {
            return Set
                    .Include(u => u.User)
                    .Where(type => type.ClaimType.Equals(claimType))
                    .ToListAsync();
        }

        public Task<List<UserClaim>> GetAllByClaimTypeAndValueAsync(string claimType, string claimValue, CancellationToken cancellationToken)
        {
            return Set
                    .Include(u => u.User)
                    .Where(type => type.ClaimType.Equals(claimType))
                    .ToListAsync(cancellationToken);
        }

        public List<UserClaim> GetAllNullUserID()
        {
            return Set.Where(x => x.UserID == null).ToList(); 
        }
    }
}
