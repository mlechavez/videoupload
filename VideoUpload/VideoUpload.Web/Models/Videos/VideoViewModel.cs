using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VideoUpload.Core;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Models.Videos
{
    public class VideoViewModel : IAsyncInitialization
    {        

        public VideoViewModel(IUnitOfWork uow, int pageNo, int pageSize)
        {                   
            Initialization = InitializeAsync(uow, pageNo, pageSize);
        }

        public VideoViewModel(IUnitOfWork uow, int pageNo, int pageSize, string search)
        {            
            Initialization = InitializeAsync(uow, pageNo, pageSize, search);
        }        
        public VideoViewModel(IUnitOfWork uow, string userID, int pageNo, int pageSize)
        {
            Initialization = InitializeAsync(uow, userID, pageNo, pageSize);         
        }

        public VideoViewModel(IUnitOfWork uow, string userID, int postID)
        {
            Initialization = InitializeAsync(uow, userID, postID);
        }
        
        public IPagedList<Post> Posts { get; private set; }
        public Post Post { get; private set; }

        public Task Initialization { get; private set; }

        private async Task InitializeAsync(IUnitOfWork uow, int pageNo, int pageSize)
        {
            var posts = await uow.Posts.GetAllAsync();
            Posts = posts.OrderByDescending(x => x.DateUploaded).ToPagedList(pageNo, pageSize);
        }
        private async Task InitializeAsync(IUnitOfWork uow, int pageNo, int pageSize, string search)
        {
            var posts = await uow.Posts.GetAllForSearchAsync(search);
            Posts = posts.ToPagedList(pageNo, pageSize);
        }
        private async Task InitializeAsync(IUnitOfWork uow, string userID, int pageNo, int pageSize)
        {
            var posts = await uow.Posts.GetAllByUserIDAsync(userID);
            Posts = posts.ToPagedList(pageNo, pageSize);
        }
        private async Task InitializeAsync(IUnitOfWork uow, string userID, int postID)
        {
            Post = await uow.Posts.GetByUserIDAndPostIDAsync(userID, postID);            
        }
    }
}