using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VideoUpload.Web.Models
{
    public class CreatePostViewModel
    {
        public CreatePostViewModel()
        {           
            DateUploaded = DateTime.UtcNow;            
        }
        public int PostID { get; set; }
        [Required]
        public string PlateNumber { get; set; }
        [Required]
        [AllowHtml]
        public string Description { get; set; }      
        public DateTime DateUploaded { get; set; }
        [Required]
        [Display(Name = "Video File")]
        public HttpPostedFileBase Attachment { get; set; }
        [Required]
        [Display(Name = "Video Image")]
        public HttpPostedFileBase VideoImage { get; set; }
    }
}