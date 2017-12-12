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
        private IActivityRepository _activities;
        private IUserClaimRepository _userClaims;
        private ICustomerRepository _customers;
        private IJobcardRepository _jobcards;
        private IHealthCheckRepository _healthChecks;
        private IHealthCheckDetailsRepository _healthCheckDetails;
        private IBranchRepository _branches;
        private IAppLogRepository _appLogs;

        #endregion

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
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

        public IActivityRepository Activities
        {
            get
            {
                return _activities ?? (_activities = new ActivityRepository(_context));
            }
        }
        public IUserClaimRepository UserClaims
        {
            get
            {
                return _userClaims ?? (_userClaims= new UserClaimRepository(_context));
            }
        }

        public ICustomerRepository Customers
        {
            get
            {
                return _customers ?? (_customers = new CustomerRepository(_context));
            }
        }

        public IJobcardRepository Jobcards
        {
            get
            {
                return _jobcards ?? (_jobcards = new JobcardRepository(_context));
            }
        }

        public IHealthCheckRepository HealthChecks
        {
            get
            {
                return _healthChecks ?? (_healthChecks = new HealthCheckRepository(_context));
            }
        }

        public IHealthCheckDetailsRepository HealthCheckDetails
        {
            get
            {
                return _healthCheckDetails ?? (_healthCheckDetails = new HealthCheckDetailsRepository(_context));
            }
        }

        public IBranchRepository Branches
        {
            get
            {
                return _branches ?? (_branches = new BranchRepository(_context));
            }
        }

        public IAppLogRepository AppLogs
        {
            get
            {
                return _appLogs ?? (_appLogs = new AppLogRepository(_context));
            }
        }

        public UnitOfWork(string nameOrConnectionString)
        {
            _context = new AppDbContext(nameOrConnectionString);
        }

        public void Dispose()
        {
            _users = null;
            _userClaims = null;
            _histories = null;
            _activities = null;           
            _posts = null;
            _attachments = null;
            _customers = null;
            _jobcards = null;
            _healthChecks = null;
            _healthCheckDetails = null;
            _branches = null;
            _appLogs = null;
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




