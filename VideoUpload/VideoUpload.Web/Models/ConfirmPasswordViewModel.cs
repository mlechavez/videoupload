using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoUpload.Web.Models
{
    public class ConfirmPasswordViewModel
    {
        public string Key { get; set; }
        public string Id { get; set; }
        public string NewPassword { get; set; }
    }
}