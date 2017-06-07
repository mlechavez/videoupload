using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.Core.Repositories
{
    public interface IUserClaimRepository : IRepository<UserClaim>
    {
        List<UserClaim> GetAllNullUserID();
        List<UserClaim> GetAllByClaimTypeAndValue(string claimType, string claimValue);
        Task<List<UserClaim>> GetAllByClaimTypeAndValueAsync(string claimType, string claimValue);
        Task<List<UserClaim>> GetAllByClaimTypeAndValueAsync(string claimType, string claimValue, CancellationToken cancellationToken);
    }
}
