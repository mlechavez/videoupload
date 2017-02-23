using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoUpload.Core;
using VideoUpload.Core.Repositories;
using VideoUpload.EF.Repositories;

namespace VideoUpload.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Private Fields
        private readonly AppDbContext _context;
        private IVideoRepository _videos;
        #endregion
        public IVideoRepository Videos
        {
            get
            {
                return _videos ?? (_videos = new VideoRepository(_context));
            }
        }
        public UnitOfWork(string nameOrConnectionString)
        {
            _context = new AppDbContext(nameOrConnectionString);
        }

        public void Dispose()
        {
            _videos = null;
            _context.Dispose();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
