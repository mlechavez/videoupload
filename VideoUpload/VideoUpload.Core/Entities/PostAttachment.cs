using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoUpload.Core.Entities
{
    public class PostAttachment
    {      
        public string PostAttachmentID { get; set; }
        public string FileName { get; set; }
        public string MIMEType { get; set; }
        public int FileSize { get; set; }
        public string FileUrl { get; set; }
        public int? PostID { get; set; }
        public string AttachmentNo { get; set; }
        public DateTime? DateCreated { get; set; }
        public string ThumbnailFileName { get; set; }
        public string ThumbnailUrl { get; set; }

        public virtual Post Post { get; set; }
    }
}
