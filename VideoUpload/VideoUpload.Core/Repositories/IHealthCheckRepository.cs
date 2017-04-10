using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.Core.Repositories
{
    public interface IHealthCheckRepository : IRepository<HealthCheck>
    {
        List<IGrouping<string, HealthCheck>> GetAllByHcGroup();
    }
}
