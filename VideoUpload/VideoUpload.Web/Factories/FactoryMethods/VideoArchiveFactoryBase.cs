using VideoUpload.Core;

namespace VideoUpload.Web.Factories.FactoryMethods
{
    public abstract class VideoArchiveFactoryBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public VideoArchiveFactoryBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected IUnitOfWork UnitOfWork {
            get { return _unitOfWork; }
        }

        public abstract IVideoArchive GetVideoArchive(string archiveBy);
    }
}