using System;
using System.Collections.Generic;

namespace VideoUpload.Core.Entities
{
    public class Post
    {
        private ICollection<PostAttachment> _attachments;
        private ICollection<History> _histories;
        private User _user;
        private Branch _branch;

        public int PostID { get; set; }
        public string PlateNumber { get; set; }        
        public virtual string Description { get; set; }                
        public DateTime DateUploaded { get; set; }
        public string EditedBy { get; set; }
        public DateTime? DateEdited { get; set; }
        public string UserID { get; set; }
        public bool HasApproval { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? DateApproved { get; set; }
        public string Approver { get; set; }
        public bool HasPlayedVideo { get; set; }
        public DateTime? DatePlayedVideo { get; set; }
        public int? BranchID { get; set; }

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

        public virtual Branch Branch
        {
            get { return _branch; }
            set { _branch = value; }
        }

        public virtual ICollection<History> Histories
        {
            get { return _histories ?? ( _histories = new List<History>()); }
            set { _histories = value; }
        }
    }
}
