using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoUpload.Core.Entities
{
    public class HealthCheckDetails
    {
        public int HealCheckDetailsID { get; set; }
        public string HcCode { get; set; }
        public string JobcardNo { get; set; }
        public bool IsGoodCondition { get; set; }
        public bool IsSuggestedToReplace { get; set; }
        public bool IsUrgentToReplace { get; set; }
        public string Comments { get; set; }
    }
}
