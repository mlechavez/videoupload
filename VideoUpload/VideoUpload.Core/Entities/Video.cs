using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoUpload.Core.Entities
{
    public class Video
    {
        public string VideoID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoFileName { get; set; }
    }
}
