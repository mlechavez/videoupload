using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.Core.Repositories
{
    public interface IPostAttachmentRepository : IRepository<PostAttachment>
    {
        PostAttachment GetByFileName(string fileName);
        Task<PostAttachment> GetByFileNameAsync(string fileName);
    }
}
