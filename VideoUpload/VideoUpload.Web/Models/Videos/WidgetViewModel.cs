using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VideoUpload.Core;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Models.Videos
{
    public class WidgetViewModel 
    {
        public WidgetViewModel(IUnitOfWork uow)
        {
            ApprovedVideos = uow.Posts.GetAllApprovedVideos(10);
            PlayedVideos = uow.Posts.GetAllPlayedVideos(10);
        }
        public List<Post> ApprovedVideos { get; private set; }        
        public List<Post> PlayedVideos { get; private set; }        
    }
}