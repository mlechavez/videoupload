using System;
using VideoUpload.Core;

namespace VideoUpload.Web.Factories.FactoryMethods
{
    public class ConcreteVideoArchiveFactory : VideoArchiveFactoryBase
    {
        private IVideoArchive _videoArchive;

        public ConcreteVideoArchiveFactory(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override IVideoArchive GetVideoArchive(string archiveBy)
        {
            switch (archiveBy)
            {                
                case "disapproved-videos":
                    _videoArchive = new DisapprovedVideosManager(UnitOfWork);
                    break;
                case "pending-approvals":
                    _videoArchive = new PendingApprovalsManager(UnitOfWork);
                    break;
                default:                
                    _videoArchive = new ApprovedVideosManager(UnitOfWork);
                    break;
            }
            return _videoArchive;
        }
    }
}