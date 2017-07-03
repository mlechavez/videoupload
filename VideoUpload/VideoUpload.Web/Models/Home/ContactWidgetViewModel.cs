using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoUpload.Core;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Models.Home
{
    public class ContactWidgetViewModel : List<Branch>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public ContactWidgetViewModel(ICollection<Branch> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = PageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)PageIndex);
            AddRange(items);
        }
        public bool HasPreviousPage
        {
            get { return PageIndex > 1; }
        }

        public bool HasNextPage
        {
            get { return PageIndex < TotalPages; }
        }
        public static ICollection<Branch> Create(IUnitOfWork uow, int pageNo, int pageSize)
        {
            var branches = uow.Branches.GetAll();
            return new ContactWidgetViewModel(branches, branches.Count, pageNo, pageSize);
        }
    }
}