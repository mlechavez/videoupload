using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;
using VideoUpload.Core.Repositories;

namespace VideoUpload.EF.Repositories
{
    internal class JobcardRepository : Repository<Jobcard>, IJobcardRepository
    {
        public JobcardRepository(AppDbContext context) : base(context)
        {
        }
    }
}
