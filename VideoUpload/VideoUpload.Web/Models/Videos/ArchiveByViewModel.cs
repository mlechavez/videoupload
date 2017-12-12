using System;
using System.Collections.Generic;
using System.Linq;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Models.Videos
{
    public class ArchiveByViewModel : ViewModelBase<IGrouping<DateTime?, Post>>
    {
        public ArchiveByViewModel(ICollection<IGrouping<DateTime?, Post>> items, int count, int pageIndex, int pageSize) : base(items, count, pageIndex, pageSize)
        {
        }
    }
}