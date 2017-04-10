using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;
using VideoUpload.Core.Repositories;

namespace VideoUpload.EF.Repositories
{
    internal class HealthCheckDetailsRepository : Repository<HealthCheckDetails>, IHealthCheckDetailsRepository
    {
        public HealthCheckDetailsRepository(AppDbContext context) : base(context)
        {
        }

        public List<HealthCheckDetails> GetAllByJobCardNo(string jobcardNo)
        {
            return Set.Where(x => x.JobcardNo == jobcardNo).ToList();
        }
    }
}
