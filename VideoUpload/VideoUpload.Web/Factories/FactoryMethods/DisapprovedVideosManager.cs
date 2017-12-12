using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoUpload.Core;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Factories.FactoryMethods
{
    public class DisapprovedVideosManager : IVideoArchive
    {
        private readonly IUnitOfWork _unitOfWork;

        public DisapprovedVideosManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<IGrouping<DateTime?, Post>>> GetArchiveAsync(int pageNo, int pageSize)
        {
            return _unitOfWork.Posts.PageByDisapprovedVideosGroupedByDayAsync(pageNo, pageSize);
        }
    }
}