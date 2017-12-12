using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VideoUpload.Core;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Factories.FactoryMethods
{
    public class PendingApprovalsManager : IVideoArchive
    {
        private readonly IUnitOfWork _unitOfWork;

        public PendingApprovalsManager(IUnitOfWork unitOfwork)
        {
            _unitOfWork = unitOfwork;
        }

        public Task<List<IGrouping<DateTime?, Post>>> GetArchiveAsync(int pageNo, int pageSize)
        {
            return _unitOfWork.Posts.PageByPendingApprovalsGroupedByDayAsync(pageNo, pageSize);
        }
    }
}