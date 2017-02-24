using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Models
{
    public class PostViewModel
    {
        public PostViewModel()
        {
            Attachments = new HashSet<PostAttachment>();
        } 
        public int PostID { get; set; }    
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Owner { get; set; }       
        public DateTime DateCreated { get; set; }
        public ICollection<PostAttachment> Attachments { get; set; }
    }
}