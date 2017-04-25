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
    internal class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(AppDbContext context) : base(context)
        {
        }             

        public IList<Post> GetAllForSearch(string v)
        {
            return Set.Where(x => x.PlateNumber.Contains(v))
                      .OrderByDescending(x => x.DateUploaded)
                      .ToList();
        }

        public IList<Post> GetAllApprovedVideos(int pageSize)
        {
            return Set.Where(x => x.IsApproved)
                      .OrderByDescending(x => x.DateUploaded)
                      .Take(pageSize)
                      .ToList();
        }

        public IList<Post> GetAllPlayedVideos(int pageSize)
        {
            return Set.Where(x => x.IsApproved && x.HasPlayedVideo)
                      .OrderByDescending(x => x.DatePlayedVideo)
                      .Take(pageSize)
                      .ToList();
        }

        public IList<Post> GetAllByUserID(string userID)
        {
            return Set.Where(x => x.User.UserID == userID)
                      .OrderByDescending(x => x.DateUploaded)
                      .ToList();
        }

        public Post GetByUserIDAndPostID(string userID, int postID)
        {
            return Set.FirstOrDefault(x => x.UserID == userID && x.PostID == postID);
        }
    }
}
