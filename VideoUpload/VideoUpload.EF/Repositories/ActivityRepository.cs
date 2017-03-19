using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;
using VideoUpload.Core.Repositories;

namespace VideoUpload.EF.Repositories
{
    internal class ActivityRepository : Repository<Activity>, IActivityRepository
    {
        public ActivityRepository(AppDbContext context) : base(context)
        {
        }

        public List<Activity> GetAllByType(string type)
        {
            return Set.Where(x => x.Type == type).ToList();
        }

        public Activity GetByTypeAndValue(string type, string value)
        {
            return Set.FirstOrDefault(x => x.Type == type && x.Value == value);
        }
    }
}
