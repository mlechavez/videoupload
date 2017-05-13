using VideoUpload.Core.Entities;
using VideoUpload.Core.Repositories;

namespace VideoUpload.EF.Repositories
{
    internal class AppLogRepository : Repository<AppLog>, IAppLogRepository
    {
        public AppLogRepository(AppDbContext context) : base(context)
        {
        }
    }
}
