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
    internal class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public User GetByEmail(string email)
        {
            return Set.FirstOrDefault(x => x.Email == email);
        }

        public Task<User> GetByEmailAsync(string email)
        {
            return Set.FirstOrDefaultAsync(x => x.Email == email);
        }

        public Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return Set.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public User GetByUsername(string userName)
        {
            return Set.FirstOrDefault(x => x.UserName == userName);
        }

        public Task<User> GetByUsernameAsync(string userName)
        {
            return Set.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public Task<User> GetByUsernameAsync(string userName, CancellationToken cancellationToken)
        {
            return Set.FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);
        }
    }
}
