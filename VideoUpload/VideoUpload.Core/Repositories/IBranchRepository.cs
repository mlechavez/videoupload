using System.Threading;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.Core.Repositories
{
    public interface IBranchRepository : IRepository<Branch>
    {
        Branch GetByName(string branchName);
        Task<Branch> GetByNameAsync(string branchName);
        Task<Branch> GetByNameAsync(string branchName, CancellationToken cancellationToken);
        
    }
}
