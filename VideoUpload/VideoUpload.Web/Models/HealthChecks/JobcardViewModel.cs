using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VideoUpload.Core;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Models.HealthChecks
{
    public class JobcardViewModel
    {
        [Required]        
        public string JobcardNo { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string ChassisNo { get; set; }
        [Required]
        public string PlateNo { get; set; }
        [Required]
        public string Mileage { get; set; }
        [Required]
        public int BranchID { get; set; }

        public ICollection<HealthCheckDetails> HealthCheckDetails { get; set; } 
    }
}