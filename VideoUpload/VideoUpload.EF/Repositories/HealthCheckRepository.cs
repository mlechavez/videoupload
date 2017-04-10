using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;
using VideoUpload.Core.Repositories;

namespace VideoUpload.EF.Repositories
{
    internal class HealthCheckRepository : Repository<HealthCheck>, IHealthCheckRepository
    {
        public HealthCheckRepository(AppDbContext context) : base(context)
        {
        }

        public List<IGrouping<string, HealthCheck>> GetAllByHcGroup()
        {
            return Set.ToList().GroupBy(x => x.HcGroup).ToList();
        }
    }
}
