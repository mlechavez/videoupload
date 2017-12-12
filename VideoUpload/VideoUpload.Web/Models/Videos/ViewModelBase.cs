using System;
using System.Collections.Generic;

namespace VideoUpload.Web.Models.Videos
{
    public abstract class ViewModelBase<T> : List<T> where T : class
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public ViewModelBase(ICollection<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = PageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public bool HasPreviousPage {
            get {
                return PageIndex > 1;
            }
        }

        public bool HasNextPage {
            get {
                return PageIndex < TotalPages;
            }
        }
    }
}