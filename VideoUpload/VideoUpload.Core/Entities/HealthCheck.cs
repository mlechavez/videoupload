using System.Collections.Generic;

namespace VideoUpload.Core.Entities
{
    public class HealthCheck
    {
        private ICollection<HealthCheckDetails> _hcDetails;

        public string HcCode { get; set; }
        public string Description { get; set; }
        public string HcGroup { get; set; }        

        public virtual ICollection<HealthCheckDetails> HealthCheckDetails
        {
            get { return _hcDetails; }
            set { _hcDetails = value; }
        }
    }
}