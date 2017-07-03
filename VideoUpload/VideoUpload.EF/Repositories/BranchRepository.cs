using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;
using VideoUpload.Core.Repositories;

namespace VideoUpload.EF.Repositories
{
    internal class BranchRepository : Repository<Branch>, IBranchRepository
    {
        public BranchRepository(AppDbContext context) : base(context)
        {
        }

        public Branch GetByName(string branchName)
        {
            return Set.FirstOrDefault(b => b.BranchName.Equals(branchName));
        }

        public Task<Branch> GetByNameAsync(string branchName)
        {
            return Set.FirstOrDefaultAsync(b => b.BranchName.Equals(branchName));
        }

        public Task<Branch> GetByNameAsync(string branchName, CancellationToken cancellationToken)
        {
            return Set.FirstOrDefaultAsync(b => b.BranchName.Equals(branchName), cancellationToken);
        }
    }
}
