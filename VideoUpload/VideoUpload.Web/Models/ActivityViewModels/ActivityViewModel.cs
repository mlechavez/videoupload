using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoUpload.Web.Models.ActivityViewModels
{
    public class ActivityViewModel
    {        
        public int ActivityID { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}