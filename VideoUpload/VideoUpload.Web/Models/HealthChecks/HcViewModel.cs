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
        }
        public HcViewModel(IUnitOfWork uow, string jobcardNo)
        {                       
            GroupedHealthChecks = uow.HealthChecks.GetAllByHcGroup();
            //HealthCheckDetails = uow.HealthCheckDetails.GetAllByJobCardNo(jobcardNo);
            var jobcard = uow.Jobcards.GetById(jobcardNo);

            Jobcard = new JobcardViewModel
            {
                CustomerName = jobcard.CustomerName,
                JobcardNo = jobcard.JobcardNo,
                ChassisNo = jobcard.ChassisNo,
                PlateNo = jobcard.PlateNo,
                Mileage = jobcard.Mileage,
                HealthCheckDetails = jobcard.HealthCheckDetails
            };
            
        }               
        [UIHint("Jobcard")] 
        public JobcardViewModel Jobcard { get; set; }   
        [UIHint("HcGroup")]             
        public List<IGrouping<string,HealthCheck>> GroupedHealthChecks { get; set; }        
        public ICollection<HealthCheckDetails> HealthCheckDetails { get; set; }      
    }
}