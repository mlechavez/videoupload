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

        public List<Post> GetByUserID(string userID)
        {
            return Set.Where(x => x.UserID == userID).ToList();
        }

        public List<Post> GetPostByApproved(int take)
        {
            return Set.Where(x => x.HasApproval && x.IsApproved).OrderByDescending(x => x.DateApproved).Take(take).ToList();
        }

        public List<Post> GetPostByVideoPlayed(int take)
        {
            return Set.Where(x => x.HasPlayedVideo).OrderByDescending(x => x.DatePlayedVideo).Take(take).ToList();
        }
    }
}
