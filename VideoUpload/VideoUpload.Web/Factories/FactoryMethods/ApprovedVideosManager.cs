using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoUpload.Core;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Factories.FactoryMethods
{
    public class ApprovedVideosManager : IVideoArchive
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApprovedVideosManager(IUnitOfWork unitOfwork)
        {
            _unitOfWork = unitOfwork;
        }

        public Task<List<IGrouping<DateTime?, Post>>> GetArchiveAsync(int pageNo, int pageSize)
        {
            return _unitOfWork.Posts.PageByApprovedVideosGroupedByDayAsync(pageNo, pageSize);    
        }
    }
}