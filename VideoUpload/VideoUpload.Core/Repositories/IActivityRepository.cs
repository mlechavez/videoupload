using System.Collections.Generic;
using VideoUpload.Core.Entities;

namespace VideoUpload.Core.Repositories
{
    public interface IActivityRepository : IRepository<Activity>
    {
        Activity GetByTypeAndValue(string type, string value);
        List<Activity> GetAllByType(string type);
    }
}
