using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VideoUpload.Web.Models.ActivityViewModels
{
    public class ActivityViewModel
    {        
        [Required]
        public int ActivityID { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public string Description { get; set; }
    }
}