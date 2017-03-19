using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.Core.Repositories
{
    public interface IActivityRepository : IRepository<Activity>
    {
        Activity GetByTypeAndValue(string type, string value);
        List<Activity> GetAllByType(string type);
    }
}
