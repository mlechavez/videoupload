using System;
using System.Collections.Generic;
using VideoUpload.Core;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Models.Videos
{
    public class VideoWidgetViewModel : List<Post>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public VideoWidgetViewModel(ICollection<Post> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = PageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageIndex);
            AddRange(items);            
        }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 1); }
        }

        public bool HasNextPage
        {
            get { return (PageIndex < TotalPages); }
        }

        public static ICollection<Post> Create(IUnitOfWork uow, int pageNo, int pageSize, string filterType, string param)
        {
            List<Post> posts = null;
            int count = 0;

            switch (filterType)
            {
                case "approved":
                    posts = uow.Posts.PageAllByApprovedVideos(pageNo - 1, pageSize);
                    count = uow.Posts.GetTotalPostsByApprovedVideos();
                    break;
                case "hasplayed":
                    posts = uow.Posts.PageAllByPlayedVideos(pageNo - 1, pageSize);
                    count = uow.Posts.GetTotalPostsByPlayedVideos();
                    break;
                default:
                    break;
            }
            return new VideoWidgetViewModel(posts, count, pageNo, pageSize);
        }
    }
}