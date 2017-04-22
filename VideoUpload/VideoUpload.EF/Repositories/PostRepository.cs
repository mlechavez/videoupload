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
      
        Task<List<Post>> IPostRepository.GetByUserIDAsync(string userID)
        {
            return Set.Where(x => x.UserID == userID).ToListAsync();
        }

        public Task<Post> GetByUserIDAndPostIDAsync(string userID, int postID)
        {
            return Set.FirstOrDefaultAsync(x => x.UserID == userID && x.PostID == postID);
        }     

        public Task<List<Post>> GetPostByApprovedAsync(int take)
        {
            return Set.Where(x => x.HasApproval && x.IsApproved).OrderByDescending(x => x.DateApproved).Take(take).ToListAsync();
        }

        public Task<List<Post>> GetPostByVideoPlayedAsync(int take)
        {
            return Set.Where(x => x.HasPlayedVideo).OrderByDescending(x => x.DatePlayedVideo).Take(take).ToListAsync();
        }

        
    }
}
