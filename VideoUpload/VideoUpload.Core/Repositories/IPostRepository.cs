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
        List<Post> GetPostByApproved(int take);
        List<Post> GetPostByVideoPlayed(int take);
        List<Post> GetByUserID(string userID);
    }
}
