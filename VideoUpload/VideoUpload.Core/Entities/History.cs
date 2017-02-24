using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoUpload.Core.Entities
{
    public class History
    {
        public int HistoryID { get; set; }
        public string Recipient { get; set; }
        public DateTime DateSent { get; set; }
        public string Sender { get; set; }
    }
}
