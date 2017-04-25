using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoUpload.Core;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Models.Videos
{
    public class VideoViewModel
    {
        public VideoViewModel(IUnitOfWork uow, int pageNo, int pageSize)
        {
            Posts = uow.Posts.GetAll().OrderByDescending(x=>x.DateUploaded).ToPagedList(pageNo, pageSize);
        }

        public VideoViewModel(IUnitOfWork uow, int pageNo, int pageSize, string search)
        {
            Posts = uow.Posts.GetAllForSearch(search).ToPagedList(pageNo, pageSize) ;
        }        
        public VideoViewModel(IUnitOfWork uow, string userID, int pageNo, int pageSize)
        {            
            Posts = uow.Posts.GetAllByUserID(userID).ToPagedList(pageNo, pageSize);            
        }

        public VideoViewModel(IUnitOfWork uow, string userID, int postID)
        {
            Post = uow.Posts.GetByUserIDAndPostID(userID, postID);
        }
        public IPagedList<Post> Posts { get; private set; }
        public Post Post { get; private set; }
    }
}