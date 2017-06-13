using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VideoUpload.Web.Models.UserViewModels
{
    public class EmailPasswordViewModel
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        [Display(Name = "Email password")]
        public string EmailPassword { get; set; }
    }
}