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
        IList<Post> GetAllByUserID(string userID);        
        IList<Post> GetAllForSearch(string v);
        IList<Post> GetAllApprovedVideos(int pageSize);
        IList<Post> GetAllPlayedVideos(int pageSize);
        Post GetByUserIDAndPostID(string userID, int postID);
    }
}
