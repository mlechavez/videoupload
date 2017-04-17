using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoUpload.Core.Entities
{
    public class HealthCheck
    {
        private HealthCheckDetails _hcDetails;

        public string HcCode { get; set; }
        public string Description { get; set; }
        public string HcGroup { get; set; }        
        public string Comments { get; set; }

        public virtual HealthCheckDetails HealthCheckDetails
        {
            get { return _hcDetails; }
            set { _hcDetails = value; }
        }
    }
}
