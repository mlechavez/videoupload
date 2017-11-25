using System.Threading;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByUsername(string userName);
        Task<User> GetByUsernameAsync(string userName);
        Task<User> GetByUsernameAsync(string userName, CancellationToken cancellationToken);

        User GetByEmail(string email);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
