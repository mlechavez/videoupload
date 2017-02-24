using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;
using VideoUpload.Core.Repositories;

namespace VideoUpload.EF.Repositories
{
    internal class PostAttachmentRepository : Repository<PostAttachment>, IPostAttachmentRepository
    {
        public PostAttachmentRepository(DbContext context) : base(context)
        {
        }

        public PostAttachment GetByFileName(string fileName)
        {
            return Set.FirstOrDefault(x => x.FileName == fileName);
        }
    }
}
