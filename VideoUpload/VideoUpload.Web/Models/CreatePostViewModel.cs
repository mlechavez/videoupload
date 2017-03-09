using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VideoUpload.Web.Models
{
    public class CreatePostViewModel
    {
        public CreatePostViewModel()
        {           
            DateUploaded = DateTimeOffset.Now;
            Attachments = new List<HttpPostedFileBase>();
        }
        public int PostID { get; set; }
        [Required]
        public string PlateNumber { get; set; }
        [Required]
        public string Description { get; set; }      
        public DateTimeOffset DateUploaded { get; set; }
        public List<HttpPostedFileBase> Attachments { get; set; }
    }
}