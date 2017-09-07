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
    public HistoryRepository(AppDbContext context) : base(context)
    {
    }

    public List<History> FindHistoriesByUserIDAndType(string userId, string sendingType)
    {
      return Set.Where(u => u.UserID == userId && u.Type == sendingType).OrderByDescending(x=>x.DateSent).ToList();
    }

    public Task<List<History>> FindHistoriesByUserIDAndTypeAsync(string userId, string sendingType)
    {
      return Set.Where(u => u.UserID == userId && u.Type == sendingType).OrderByDescending(x => x.DateSent).ToListAsync();
    }
  }
}
