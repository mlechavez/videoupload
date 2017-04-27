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
        Task<List<Post>> GetAllByUserIDAsync(string userID);
        Task<List<Post>> GetAllForSearchAsync(string v);
        List<Post> GetAllApprovedVideos(int pageSize);        
        Task<List<Post>> GetAllApprovedVideosAsync(int pageSize);
        List<Post> GetAllPlayedVideos(int pageSize);
        Task<List<Post>> GetAllPlayedVideosAsync(int pageSize);
        Task<Post> GetByUserIDAndPostIDAsync(string userID, int postID);
    }
}
