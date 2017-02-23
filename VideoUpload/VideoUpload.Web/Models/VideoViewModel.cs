using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VideoUpload.Web.Models
{
    public class VideoViewModel
    {
        public VideoViewModel()
        {
            VideoID = Guid.NewGuid().ToString();
            Videos = new List<HttpPostedFileBase>();
        }
        
        public string VideoID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        
        public string VideoFileName { get; set; }
        [DataType(DataType.Upload)]
        public List<HttpPostedFileBase> Videos { get; set; }
    }
}