using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VideoUpload.Core;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Models.HealthChecks
{
    public class CreateHcViewModel
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
        public string A01 { get; set; }
        [Required]
        public string A02 { get; set; }
        [Required]
        public string A03 { get; set; }
        [Required]
        public string A04 { get; set; }
        [Required]
        public string A05 { get; set; }
        [Required]
        public string A06 { get; set; }
        [Required]
        public string A07 { get; set; }
        [Required]
        public string A08 { get; set; }
        [Required]
        public string A09 { get; set; }
        [Required]
        public string A10 { get; set; }
        [Required]
        public string A11 { get; set; }
        [Required]
        public string A12 { get; set; }
        [Required]
        public string A14 { get; set; }
        [Required]
        public string B01 { get; set; }
        [Required]
        public string B02 { get; set; }
        [Required]
        public string B03 { get; set; }
        [Required]
        public string B04 { get; set; }
        [Required]
        public string B05 { get; set; }
        [Required]
        public string B06 { get; set; }
        [Required]
        public string C01 { get; set; }
        [Required]
        public string C02 { get; set; }
        [Required]
        public string C03 { get; set; }
        [Required]
        public string C04 { get; set; }
        [Required]
        public string C05 { get; set; }
        [Required]
        public string C06 { get; set; }
        [Required]
        public string C07 { get; set; }
        [Required]
        public string C08 { get; set; }
        [Required]
        public string C09 { get; set; }
        [Required]
        public string D01 { get; set; }
        [Required]
        public string D04 { get; set; }
        [Required]
        public string D05 { get; set; }
        [Required]
        public string E01 { get; set; }
        [Required]
        public string E02 { get; set; }
        [Required]
        public string E03 { get; set; }
        [Required]
        public string E04 { get; set; }                
        public ICollection<IGrouping<string, HealthCheck>> GroupedHealthChecks { get; set; }
        public ICollection<HealthCheckDetails> MyProperty { get; set; }
    }
}