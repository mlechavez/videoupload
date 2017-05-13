using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoUpload.Core.Entities
{
    public class AppLog
    {
        public int AppLogID { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
        public DateTime LogDate { get; set; }
        public string UserName { get; set; }
    }
}
