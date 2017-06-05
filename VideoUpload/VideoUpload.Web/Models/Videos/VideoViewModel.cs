using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoUpload.Core;
using VideoUpload.Core.Entities;
using VideoUpload.Web.Common;

namespace VideoUpload.Web.Models.Videos
{
    public class VideoViewModel : List<Post>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public VideoViewModel(ICollection<Post> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

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

        public static async Task<ICollection<Post>> CreateAsync(IUnitOfWork uow, int pageIndex, int pageSize, string filterType, string param)
        {
            List<Post> posts;
            int count = 0;

            switch (filterType)
            {
                case "user":
                    posts = await uow.Posts.PageAllByUserIDAsync(param, pageIndex - 1, pageSize);
                    count = await uow.Posts.GetTotalPostsByUserIDAsync(param);
                    break;
                case "search":
                    posts = await uow.Posts.PageAllBySearchAsync(param, pageIndex - 1, pageSize);
                    count = await uow.Posts.GetTotalPostsBySearchAsync(param);
                    break;                
                default:
                    posts = await uow.Posts.PageAllByPostedOnAsync(pageIndex - 1, pageSize);
                    count = await uow.Posts.GetTotalPostsAsync();
                    break;
            }

            return new VideoViewModel(posts, count, pageIndex, pageSize);
        }        
    }
}