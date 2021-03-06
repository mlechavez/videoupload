﻿using System;
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
            Attachments = new List<HttpPostedFileBase>();
        }
        public int PostID { get; set; }
        [Required]
        public string PlateNumber { get; set; }
        [Required]
        [AllowHtml]
        public string Description { get; set; }      
        public DateTime DateUploaded { get; set; }
        public List<HttpPostedFileBase> Attachments { get; set; }
    }
}