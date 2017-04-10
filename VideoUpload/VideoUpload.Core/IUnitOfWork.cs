using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoUpload.Core.Repositories;

namespace VideoUpload.Core
{
    public interface IUnitOfWork : IDisposable
    {               
        IPostRepository Posts { get; }
        IPostAttachmentRepository Attachments { get; }
        IHistoryRepository Histories { get; }
        IUserRepository Users { get; }
        IUserClaimRepository UserClaims { get; }
        IActivityRepository Activities { get; }
        ICustomerRepository Customers { get; }
        IJobcardRepository Jobcards { get; }
        IHealthCheckRepository HealthChecks { get; }
        IHealthCheckDetailsRepository HealthCheckDetails { get; }
        IBranchRepository Branches { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
