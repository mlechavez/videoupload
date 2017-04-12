using System;
using System.Collections.Generic;
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
            HealthCheckDetails = new List<HealthCheckDetails>();
        }
        public HcViewModel(IUnitOfWork uow)
        {
            GroupedHealthChecks = uow.HealthChecks.GetAllByHcGroup();
            HealthCheckDetails = new List<HealthCheckDetails>();        
        }
        public HcViewModel(IUnitOfWork uow, string jobcardNo)
        {                       
            GroupedHealthChecks = uow.HealthChecks.GetAllByHcGroup();
            HealthCheckDetails = uow.HealthCheckDetails.GetAllByJobCardNo(jobcardNo);
        }
        
        public Jobcard Jobcard { get; set; }        
        public List<IGrouping<string,HealthCheck>> GroupedHealthChecks { get; set; }
        public List<HealthCheckDetails> HealthCheckDetails { get; set; }
    }
}