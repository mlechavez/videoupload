using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoUpload.Core.Entities
{
    public class Post
    {
        private ICollection<PostAttachment> _attachments;

        public int PostID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }        
        public string Owner { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ICollection<PostAttachment> Attachments
        {
            get { return _attachments ?? (_attachments = new List<PostAttachment>()); }
            set { _attachments = value; }
        }
    }
}
