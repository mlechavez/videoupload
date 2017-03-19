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
        private User _user;

        public int PostID { get; set; }
        public string PlateNumber { get; set; }
        public string Description { get; set; }                
        public DateTime DateUploaded { get; set; }
        public string EditedBy { get; set; }
        public DateTime? DateEdited { get; set; }
        public string UserID { get; set; }
        public bool HasApproval { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? DateApproved { get; set; }
        public bool HasPlayedVideo { get; set; }
        public DateTime? DatePlayedVideo { get; set; }

        public virtual ICollection<PostAttachment> Attachments
        {
            get { return _attachments ?? (_attachments = new List<PostAttachment>()); }
            set { _attachments = value; }
        }
        public virtual User User
        {
            get { return _user; }
            set { _user = value; }
        }
    }
}
