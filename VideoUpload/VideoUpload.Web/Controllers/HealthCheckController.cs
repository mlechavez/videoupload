using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using VideoUpload.Core;
using VideoUpload.Core.Entities;
using VideoUpload.Web.Models.HealthChecks;

namespace VideoUpload.Web.Controllers
{
    [RoutePrefix("health-check")]
    public class HealthCheckController : Controller
    {
        private readonly IUnitOfWork _uow;

        public HealthCheckController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload(HttpPostedFileBase file)
        {
            var isSuccess = false;
            if (file != null && file.ContentLength > 0)
            {
                isSuccess = UploadCsvToDatabase(file.InputStream);
            }

            if (isSuccess)
            {

            }
            return View();
        }

        public ActionResult Details(string jobcardNo)
        {
            var viewModel = new HcViewModel(_uow, jobcardNo);

            return View(viewModel);
        }

        private bool UploadCsvToDatabase(Stream inputStream)
        {
            var isSuccess = false;
            var line = string.Empty;
            string[] strArray;
            var jobcardNo = string.Empty;
            var customerName = string.Empty;
            var chassisNo = string.Empty;
            var plateNo = string.Empty;
            var mileage = string.Empty;
            var hcCode = string.Empty;
            var isGoodCondition = false;
            var isSuggestedToReplace = false;
            var isUrgentToReplace = false;

            //We should not seperate the comma in a sentence
            Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            StreamReader sr = new StreamReader(inputStream);

            //read the first row which the column header
            line = sr.ReadLine();
            strArray = r.Split(line);

            var jobcard = new Jobcard();            
            var healthCheckDetails = new HealthCheckDetails();

            while ((line = sr.ReadLine()) != null)
            {
                strArray = r.Split(line);

                jobcardNo = !string.IsNullOrWhiteSpace(strArray[0]) ? strArray[0].ToString() : string.Empty;
                customerName = !string.IsNullOrWhiteSpace(strArray[1]) ? strArray[1].ToString() : string.Empty;
                chassisNo = !string.IsNullOrWhiteSpace(strArray[2]) ? strArray[2].ToString() : string.Empty;
                plateNo = !string.IsNullOrWhiteSpace(strArray[3]) ? strArray[3].ToString() : string.Empty;
                mileage = !string.IsNullOrWhiteSpace(strArray[4]) ? strArray[4].ToString() : string.Empty;
                hcCode = !string.IsNullOrWhiteSpace(strArray[5]) ? strArray[5].ToString() : string.Empty;

                if (strArray[6].ToString().ToLower() == "t")
                {
                    isGoodCondition = true;
                }

                if (strArray[7].ToString().ToLower() == "t")
                {
                    isSuggestedToReplace = true;
                }

                if (strArray[8].ToString().ToLower() == "t")
                {
                    isUrgentToReplace = true;
                }

                jobcard.JobcardNo = jobcardNo;
                jobcard.CustomerName = customerName;
                jobcard.ChassisNo = chassisNo;
                jobcard.PlateNo = plateNo;
                jobcard.Mileage = mileage;

                _uow.Jobcards.Add(jobcard);

                healthCheckDetails.JobcardNo = jobcard.JobcardNo;
                healthCheckDetails.HcCode = hcCode;
                healthCheckDetails.IsGoodCondition = isGoodCondition;
                healthCheckDetails.IsSuggestedToReplace = isSuggestedToReplace;
                healthCheckDetails.IsUrgentToReplace = isUrgentToReplace;

                _uow.HealthCheckDetails.Add(healthCheckDetails);
            }
            
            try
            {                
                _uow.SaveChanges();
                return isSuccess = true;
            }
            catch (Exception ex)
            {
                return isSuccess;              
            }   
            finally
            {
                sr.Dispose();
            }                     
        }
    }
}