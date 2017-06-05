using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.Core.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Post GetByDateUploadedAndPostID(int year, int month, int postID);
        Task<Post> GetByDateUploadedAndPostIDAsync(int year, int month, int postID);
        Task<Post> GetByDateUploadedAndPostIDAsync(int year, int month, int postID, CancellationToken cancellationToken);        

        List<Post> PageAllByApprovedVideos(int pageSize);        
        Task<List<Post>> PageAllByApprovedVideosAsync(int pageSize);

        List<Post> PageAllByPlayedVideos(int pageSize);
        Task<List<Post>> PageAllByPlayedVideosAsync(int pageSize);

        Task<Post> GetByUserIDAndPostIDAsync(string userID, int postID);


        List<Post> PageAllByPostedOn(int pageNo, int pageSize);
        Task<List<Post>> PageAllByPostedOnAsync(int pageNo, int pageSize);
        Task<List<Post>> PageAllByPostedOnAsync(int pageNo, int pageSize, CancellationToken cancellationToken);
        int GetTotalPosts();
        Task<int> GetTotalPostsAsync();

        List<Post> PageAllByUserID(string userID, int pageNo, int pageSize);
        Task<List<Post>> PageAllByUserIDAsync(string userID, int pageNo, int pageSize);
        Task<List<Post>> PageAllByUserIDAsync(string userID, int pageNo, int pageSize, CancellationToken cancellationToken);
        int GetTotalPostsByUserID(string userID);
        Task<int> GetTotalPostsByUserIDAsync(string userID);

        List<Post> PageAllBySearch(string s, int pageNo, int pageSize);
        Task<List<Post>> PageAllBySearchAsync(string s, int pageNo, int pageSize);
        Task<List<Post>> PageAllBySearchAsync(string s, int pageNo, int pageSize, CancellationToken cancellationToken);
        int GetTotalPostsBySearch(string s);
        Task<int> GetTotalPostsBySearchAsync(string s);
    }
}
