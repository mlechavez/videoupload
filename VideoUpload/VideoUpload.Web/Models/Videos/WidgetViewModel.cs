using System.Collections.Generic;
using VideoUpload.Core;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Models.Videos
{
    public class WidgetViewModel 
    {
        public WidgetViewModel(IUnitOfWork uow)
        {
            ApprovedVideos = uow.Posts.PageAllByApprovedVideos(10);
            PlayedVideos = uow.Posts.PageAllByPlayedVideos(10);
        }
        public List<Post> ApprovedVideos { get; private set; }        
        public List<Post> PlayedVideos { get; private set; }        
    }
}