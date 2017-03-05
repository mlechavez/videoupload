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
        private IUserRepository _users;
        private IPostRepository _posts;
        private IPostAttachmentRepository _attachments;
        private IHistoryRepository _histories;
        #endregion
        public IPostRepository Posts
        {
            get
            {
                return _posts ?? (_posts = new PostRepository(_context));
            }
        }      
        public IPostAttachmentRepository Attachments
        {
            get
            {
                return _attachments ?? (_attachments = new PostAttachmentRepository(_context));
            }
        }

        public IHistoryRepository Histories
        {
            get
            {
                return _histories ?? (_histories = new HistoryRepository(_context));
            }
        }

        public IUserRepository Users
        {
            get
            {
                return _users ?? (_users = new UserRepository(_context));
            }
        }

        public UnitOfWork(string nameOrConnectionString)
        {
            _context = new AppDbContext(nameOrConnectionString);
        }

        public void Dispose()
        {
            _posts = null;
            _attachments = null;
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
