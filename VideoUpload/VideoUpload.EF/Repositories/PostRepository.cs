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

        public Task<List<Post>> GetAllForSearchAsync(string v)
        {
            return Set.Where(x => x.PlateNumber.Contains(v))
                      .OrderByDescending(x => x.DateUploaded)
                      .ToListAsync();
        }

        public Task<List<Post>> GetAllApprovedVideosAsync(int pageSize)
        {
            return Set.Where(x => x.IsApproved)
                      .OrderByDescending(x => x.DateUploaded)
                      .Take(pageSize)
                      .ToListAsync();
        }

        public Task<List<Post>> GetAllPlayedVideosAsync(int pageSize)
        {
            return Set.Where(x => x.IsApproved && x.HasPlayedVideo)
                      .OrderByDescending(x => x.DatePlayedVideo)
                      .Take(pageSize)
                      .ToListAsync();
        }

        public Task<List<Post>> GetAllByUserIDAsync(string userID)
        {
            return Set.Where(x => x.User.UserID == userID)
                      .OrderByDescending(x => x.DateUploaded)
                      .ToListAsync();
        }

        public Task<Post> GetByUserIDAndPostIDAsync(string userID, int postID)
        {
            return Set.FirstOrDefaultAsync(x => x.UserID == userID && x.PostID == postID);
        }

        public List<Post> GetAllApprovedVideos(int pageSize)
        {
            return Set.Where(x => x.IsApproved)
                      .OrderByDescending(x => x.DateUploaded)
                      .Take(pageSize)
                      .ToList();
        }

        public List<Post> GetAllPlayedVideos(int pageSize)
        {
            return Set.Where(x => x.IsApproved && x.HasPlayedVideo)
                      .OrderByDescending(x => x.DatePlayedVideo)
                      .Take(pageSize)
                      .ToList();
        }
    }
}
