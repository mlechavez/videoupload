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
        [Display(Name = "Plate number")]
        public string PlateNumber { get; set; }
        [Required]
        public string Description { get; set; }               
        public DateTime DateUploaded { get; set; }
        [Required]
        public string UploadedBy { get; set; }
        public string EditedBy { get; set; }
        public DateTime? DateEdited { get; set; }
        public bool IsApproved { get; set; }

        public ICollection<PostAttachment> Attachments { get; set; }
    }
}