using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoUpload.Web.Models
{
    public class AttachmentViewModel
    {
        public AttachmentViewModel()
        {
            PostAttachmentID = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
        }
        public string PostAttachmentID { get; set; }
        public string FileName { get; set; }
        public string MIMEType { get; set; }
        public int FileSize { get; set; }
        public string PostID { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}