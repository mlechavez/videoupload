using System.Collections.Generic;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.Core.Repositories
{
  public interface IHistoryRepository : IRepository<History>
  {
    List<History> FindHistoriesByUserIDAndType(string userId, string sendingType);
    Task<List<History>> FindHistoriesByUserIDAndTypeAsync(string userId, string sendingType);
  }
}
