using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoUpload.Core.Entities
{
    public class HealthCheckDetails
    {
        private HealthCheck _hc;
        private Jobcard _jobcard;

        public int HealCheckDetailsID { get; set; }
        public string HcCode { get; set; }
        public string JobcardNo { get; set; }
        public string Status { get; set; }       
        public string Comments { get; set; }

        public virtual HealthCheck HealthCheck
        {
            get { return _hc; }
            set { _hc = value; }
        }

        public virtual Jobcard Jobcard
        {
            get { return _jobcard; }
            set { _jobcard = value; }
        }
    }
}