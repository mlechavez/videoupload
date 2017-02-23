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
        IVideoRepository Videos { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
