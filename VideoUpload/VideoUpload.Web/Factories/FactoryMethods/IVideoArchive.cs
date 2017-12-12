using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Factories.FactoryMethods
{
    public interface IVideoArchive
    {
        Task<List<IGrouping<DateTime?, Post>>> GetArchiveAsync(int pageNo, int pageSize);
    }
}
