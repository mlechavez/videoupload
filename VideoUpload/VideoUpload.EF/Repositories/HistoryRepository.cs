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
    internal class HistoryRepository : Repository<History>, IHistoryRepository
    {
        public HistoryRepository(DbContext context) : base(context)
        {
        }
    }
}
