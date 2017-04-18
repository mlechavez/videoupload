using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoUpload.Core.Entities
{
    public class Jobcard
    {
        private Branch _branch;
        private ICollection<HealthCheckDetails> _hcDetails;

        public string JobcardNo { get; set; }
        public string CustomerName { get; set; }
        public string ChassisNo { get; set; }
        public string PlateNo { get; set; }
        public string Mileage { get; set; }
        public int? BranchID { get; set; }

        public virtual Branch Branch
        {
            get { return _branch; }
            set { _branch = value; }
        }
        public virtual ICollection<HealthCheckDetails> HealthCheckDetails
        {
            get { return _hcDetails ?? (_hcDetails = new List<HealthCheckDetails>()); }
            set { _hcDetails = value; }
        }


    }
}
