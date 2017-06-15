using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
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

        public Post GetByDateUploadedAndPostID(int year, int month, int postID)
        {
            return Set
                    .Include(u => u.User)
                    .Include(b => b.Branch)
                    .Include(attch => attch.Attachments)
                    .Include(history => history.Histories)
                    .OrderByDescending(date => date.DateUploaded)
                    .FirstOrDefault(post => post.DateUploaded.Year.Equals(year) &&
                        post.DateUploaded.Month.Equals(month) &&
                        post.PostID.Equals(postID));                    
        }

        public Task<Post> GetByDateUploadedAndPostIDAsync(int year, int month, int postID)
        {
            return Set
                    .Include(u => u.User)
                    .Include(b => b.Branch)
                    .Include(attch => attch.Attachments)
                    .Include(history => history.Histories)                    
                    .FirstOrDefaultAsync(post => post.DateUploaded.Year.Equals(year) &&
                        post.DateUploaded.Month.Equals(month) &&
                        post.PostID.Equals(postID));
        }

        public Task<Post> GetByDateUploadedAndPostIDAsync(int year, int month, int postID, CancellationToken cancellationToken)
        {
            return Set
                    .Include(u => u.User)
                    .Include(b => b.Branch)
                    .Include(attch => attch.Attachments)
                    .Include(history => history.Histories)                    
                    .FirstOrDefaultAsync(post => post.DateUploaded.Year.Equals(year) &&
                        post.DateUploaded.Month.Equals(month) &&
                        post.PostID.Equals(postID), cancellationToken);
        }
        
        public List<Post> PageAllByApprovedVideos(int pageNo, int pageSize)
        {
            return Set.Where(x => x.IsApproved)
                       .OrderByDescending(x => x.DateUploaded)
                       .Take(pageSize)
                       .ToList();
        }

        public int GetTotalPostsByApprovedVideos()
        {
            return Set
                    .Where(approved => approved.IsApproved)
                    .Count();
        }

        public Task<int> GetTotalPostsByApprovedVideosAsync()
        {
            return Set
                    .Where(approved => approved.IsApproved)
                    .CountAsync();
        }

        public Task<List<Post>> PageAllByApprovedVideosAsync(int pageNo, int pageSize)
        {
            return Set.Where(x => x.IsApproved)
                      .OrderByDescending(x => x.DateUploaded)
                      .Skip(pageNo * pageSize)
                      .Take(pageSize)
                      .ToListAsync();
        }
        public List<Post> PageAllByPlayedVideos(int pageNo, int pageSize)
        {
            return Set.Where(x => x.IsApproved && x.HasPlayedVideo)
                      .OrderByDescending(x => x.DatePlayedVideo)
                      .Skip(pageNo * pageSize)
                      .Take(pageSize)
                      .ToList();
        }

        public Task<List<Post>> PageAllByPlayedVideosAsync(int pageNo, int pageSize)
        {
            return Set.Where(x => x.IsApproved && x.HasPlayedVideo)
                      .OrderByDescending(x => x.DatePlayedVideo)
                      .Take(pageSize)
                      .ToListAsync();
        }
        public int GetTotalPostsByPlayedVideos()
        {
            return Set
                    .Where(video => video.HasPlayedVideo)
                    .Count();
        }
        public Task<int> GetTotalPostsByPlayedVideosAsync()
        {
            return Set
                    .Where(video => video.HasPlayedVideo)
                    .CountAsync();
        }

        public Task<Post> GetByUserIDAndPostIDAsync(string userID, int postID)
        {
            return Set                    
                    .FirstOrDefaultAsync(x => x.UserID == userID && x.PostID == postID);
        }               

        public List<Post> PageAllByPostedOn(int pageNo, int pageSize)
        {
            return Set
                    .Include(u => u.User)
                    .Include(b => b.Branch)
                    .Include(attch => attch.Attachments)
                    .Include(history => history.Histories)
                    .OrderByDescending(date => date.DateUploaded)
                    .Skip(pageNo * pageSize)
                    .Take(pageSize)                    
                    .ToList();

        }

        public Task<List<Post>> PageAllByPostedOnAsync(int pageNo, int pageSize)
        {
            return Set
                    .Include(u => u.User)
                    .Include(b => b.Branch)
                    .Include(attch => attch.Attachments)
                    .Include(history => history.Histories)
                    .OrderByDescending(date => date.DateUploaded)
                    .Skip(pageNo * pageSize)
                    .Take(pageSize)                    
                    .ToListAsync();
        }

        public Task<List<Post>> PageAllByPostedOnAsync(int pageNo, int pageSize, CancellationToken cancellationToken)
        {
            return Set
                    .Include(u => u.User)
                    .Include(b => b.Branch)
                    .Include(attch => attch.Attachments)
                    .Include(history => history.Histories)
                    .OrderByDescending(date => date.DateUploaded)
                    .Skip(pageNo * pageSize)
                    .Take(pageSize)                    
                    .ToListAsync(cancellationToken);
        }

        public int GetTotalPosts()
        {
            return Set.Count();
        }

        public Task<int> GetTotalPostsAsync()
        {
            return Set.CountAsync();
        }

        public List<Post> PageAllByUserID(string userID, int pageNo, int pageSize)
        {
            return Set
                    .Include(u => u.User)
                    .Include(b => b.Branch)
                    .Include(attch => attch.Attachments)
                    .Include(history => history.Histories)
                    .Where(user => user.UserID.Equals(userID))
                    .OrderByDescending(date => date.DateUploaded)
                    .Skip(pageNo * pageSize)
                    .Take(pageSize)                    
                    .ToList();
        }
        public Task<List<Post>> PageAllByUserIDAsync(string userID, int pageNo, int pageSize)
        {
            return Set
                    .Include(u => u.User)
                    .Include(b => b.Branch)
                    .Include(attch => attch.Attachments)
                    .Include(history => history.Histories)
                    .Where(user => user.UserID.Equals(userID))
                    .OrderByDescending(date => date.DateUploaded)
                    .Skip(pageNo * pageSize)
                    .Take(pageSize)                    
                    .ToListAsync();
        }

        public Task<List<Post>> PageAllByUserIDAsync(string userID, int pageNo, int pageSize, CancellationToken cancellationToken)
        {
            return Set
                    .Include(u => u.User)
                    .Include(b => b.Branch)
                    .Include(attch => attch.Attachments)
                    .Include(history => history.Histories)
                    .Where(user => user.UserID.Equals(userID))
                    .OrderByDescending(date => date.DateUploaded)
                    .Skip(pageNo * pageSize)
                    .Take(pageSize)                    
                    .ToListAsync(cancellationToken);
        }

        public int GetTotalPostsByUserID(string userID)
        {
            return Set
                    .Where(user => user.UserID.Equals(userID))
                    .Count();
        }

        public Task<int> GetTotalPostsByUserIDAsync(string userID)
        {
            return Set
                    .Where(user => user.UserID.Equals(userID))
                    .CountAsync();
        }

        public List<Post> PageAllBySearch(string s, int pageNo, int pageSize)
        {
            return Set
                    .Include(u => u.User)
                    .Include(b => b.Branch)
                    .Include(attch => attch.Attachments)
                    .Include(history => history.Histories)
                    .Where(post => post.PlateNumber.Contains(s) ||
                       post.User.UserName.Contains(s))
                    .OrderByDescending(date => date.DateUploaded)
                    .Skip(pageNo * pageSize)
                    .Take(pageSize)                    
                    .ToList();
        }

        public Task<List<Post>> PageAllBySearchAsync(string s, int pageNo, int pageSize)
        {
            return Set
                    .Include(u => u.User)
                    .Include(b => b.Branch)
                    .Include(attch => attch.Attachments)
                    .Include(history => history.Histories)
                    .Where(post => post.PlateNumber.Contains(s) ||
                       post.User.UserName.Contains(s))
                    .OrderByDescending(date => date.DateUploaded)
                    .Skip(pageNo * pageSize)
                    .Take(pageSize)                    
                    .ToListAsync();
        }

        public Task<List<Post>> PageAllBySearchAsync(string s, int pageNo, int pageSize, CancellationToken cancellationToken)
        {
            return Set
                    .Include(u => u.User)
                    .Include(b => b.Branch)
                    .Include(attch => attch.Attachments)
                    .Include(history => history.Histories)
                    .Where(post => post.PlateNumber.Contains(s) ||
                       post.User.UserName.Contains(s))
                    .OrderByDescending(date => date.DateUploaded)
                    .Skip(pageNo * pageSize)
                    .Take(pageSize)                    
                    .ToListAsync(cancellationToken);
        }

        public int GetTotalPostsBySearch(string s)
        {
            return Set
                    .Where(post => post.PlateNumber.Contains(s) ||
                       post.User.UserName.Contains(s))
                    .Count();
        }

        public Task<int> GetTotalPostsBySearchAsync(string s)
        {
            return Set
                    .Where(post => post.PlateNumber.Contains(s) ||
                       post.User.UserName.Contains(s))
                    .CountAsync();
        }

        public List<Post> GetAllForApprovalsByBranchID(int branchID)
        {
            return Set
                    .Where(post => !post.HasApproval && post.BranchID.Equals(branchID))
                    .ToList();
        }

        public Task<List<Post>> GetAllForApprovalsByBranchIDAsync(int branchID)
        {
            return Set
                    .Where(post => !post.HasApproval && post.BranchID.Equals(branchID))
                    .ToListAsync();
        }

        public Task<List<Post>> GetAllForApprovalsByBranchIDAsync(int branchID, CancellationToken cancellationToken)
        {
            return Set
                    .Where(post => !post.HasApproval && post.BranchID.Equals(branchID))
                    .ToListAsync(cancellationToken);
        }
    }
}
