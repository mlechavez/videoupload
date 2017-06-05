using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoUpload.Core;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Models.Videos
{
    public class WidgetViewModel : List<Post>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public WidgetViewModel(ICollection<Post> items, int count, int pageIndex, int pageSize)
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

        public static async Task<ICollection<Post>> CreateAsync(IUnitOfWork uow, int pageNo, int pageSize, string filterType, string param)
        {
            List<Post> posts = null;
            int count = 0;

            switch (filterType)
            {
                case "approved":
                    posts = await uow.Posts.PageAllByApprovedVideosAsync(pageNo - 1, pageSize);
                    count = await uow.Posts.GetTotalPostsByApprovedVideosAsync();
                    break;
                case "hasplayed":
                    posts = await uow.Posts.PageAllByPlayedVideosAsync(pageNo - 1, pageSize);
                    count = await uow.Posts.GetTotalPostsByPlayedVideosAsync();
                    break;
                default:
                    break;
            }
            return new WidgetViewModel(posts, count, pageNo, pageSize);
        }
    }
}