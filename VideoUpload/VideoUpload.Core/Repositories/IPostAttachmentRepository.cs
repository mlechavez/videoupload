using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.Core.Repositories
{
    public interface IPostAttachmentRepository : IRepository<PostAttachment>
    {
        PostAttachment GetByFileName(string fileName);
    }
}
