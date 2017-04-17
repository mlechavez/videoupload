using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VideoUpload.Core;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Models.HealthChecks
{
    public class HcViewModel
    {
        
        public HcViewModel()
        {            
        }
        public HcViewModel(IUnitOfWork uow)
        {            
            HealthCheckDetails = new List<HealthCheckDetails>();
            GroupedHealthChecks = uow.HealthChecks.GetAllByHcGroup();

            //GroupedHealthChecks.ForEach(hc =>
            //{
            //    hc.ToList().ForEach(hcDetails =>
            //    {                    
            //        HealthCheckDetails.Add(new HealthCheckDetails
            //        {
            //            HcCode = hcDetails.HcCode                        
            //        });
            //    });
            //});
        }
        public HcViewModel(IUnitOfWork uow, string jobcardNo)
        {                       
            GroupedHealthChecks = uow.HealthChecks.GetAllByHcGroup();            
        }                
        public JobcardViewModel Jobcard { get; set; }   
        [UIHint("HcGroup")]             
        public List<IGrouping<string,HealthCheck>> GroupedHealthChecks { get; set; }        
        public ICollection<HealthCheckDetails> HealthCheckDetails { get; set; }      
    }
}