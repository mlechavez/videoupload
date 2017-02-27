﻿using System;
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
            DateCreated = DateTime.Now;
            Attachments = new List<HttpPostedFileBase>();
        }
        public int PostID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Owner { get; set; }
        public DateTime DateCreated { get; set; }
        public List<HttpPostedFileBase> Attachments { get; set; }
    }
}