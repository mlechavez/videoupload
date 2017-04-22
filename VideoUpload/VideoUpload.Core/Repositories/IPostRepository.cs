using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.Core.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<List<Post>> GetPostByApprovedAsync(int take);
        Task<List<Post>> GetPostByVideoPlayedAsync(int take);
        Task<List<Post>> GetByUserIDAsync(string userID);
        Task<Post> GetByUserIDAndPostIDAsync(string userID, int postID);
    }
}
