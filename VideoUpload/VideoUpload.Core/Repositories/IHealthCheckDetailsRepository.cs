using System.Collections.Generic;
using VideoUpload.Core.Entities;

namespace VideoUpload.Core.Repositories
{
    public interface IHealthCheckDetailsRepository : IRepository<HealthCheckDetails>
    {
        List<HealthCheckDetails> GetAllByJobCardNo(string jobcardNo);
    }
}
