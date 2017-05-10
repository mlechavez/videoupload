using System;

namespace VideoUpload.Core.Entities
{
    public class History
    {
        private User _user;
        private Post _post;

        public int HistoryID { get; set; }
        public string Recipient { get; set; }
        public string Name { get; set; }
        public DateTime DateSent { get; set; }
        public string UserID { get; set; }
        public string Type { get; set; }
        public int PostID { get; set; }

        public virtual User User
        {
            get { return _user; }
            set { _user = value; }
        }

        public virtual Post Post
        {
            get { return _post; }
            set { _post = value; }
        }
    }
}
