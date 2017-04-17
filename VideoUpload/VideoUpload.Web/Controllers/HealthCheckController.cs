using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using VideoUpload.Core;
using VideoUpload.Core.Entities;
using VideoUpload.Web.Common;
using VideoUpload.Web.Models.HealthChecks;

namespace VideoUpload.Web.Controllers
{
    [RoutePrefix("health-check")]
    public class HealthCheckController : AppController
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

        public ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase csv)
        {
            var isSuccess = false;
            
            if (csv != null && csv.ContentLength > 0)
            {
                isSuccess = UploadCsvToDatabase(csv.InputStream);
            }

            if (isSuccess)
            {

            }
            return View();
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
            var comments = string.Empty;

            //We should not seperate the comma in a sentence
            Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            StreamReader sr = new StreamReader(inputStream);

            //read the first row which the column header
            line = sr.ReadLine();
            strArray = r.Split(line);

            var jobcard = new Jobcard();           

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

                comments = !string.IsNullOrWhiteSpace(strArray[9]) ? strArray[9].ToString() : string.Empty;                               

                _uow.HealthCheckDetails.Add(new HealthCheckDetails
                {
                    JobcardNo = jobcardNo,
                    HcCode = hcCode,
                    
                    //IsGoodCondition = isGoodCondition,
                    //IsSuggestedToReplace = isSuggestedToReplace,
                    //IsUrgentToReplace = isUrgentToReplace,
                    Comments = comments
                });               
            }

            jobcard.JobcardNo = jobcardNo;
            jobcard.CustomerName = customerName;
            jobcard.ChassisNo = chassisNo;
            jobcard.PlateNo = plateNo;
            jobcard.Mileage = mileage;
            jobcard.BranchID = CurrentUser.BranchID; //inherit the AppController to get the CurrentUser Property
            _uow.Jobcards.Add(jobcard);

            _uow.SaveChanges();
            sr.Dispose();
            isSuccess = true;
            return isSuccess;
        }

        public ActionResult Details(string jobcardNo)
        {
            var viewModel = new HcViewModel(_uow, jobcardNo);

            return View(viewModel);
        }

        public ActionResult New()
        {
            var viewModel = new HcViewModel(_uow);            
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult New(HcViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var jobcard = new Jobcard
                {
                    JobcardNo = viewModel.Jobcard.JobcardNo,
                    CustomerName = viewModel.Jobcard.CustomerName,
                    ChassisNo = viewModel.Jobcard.ChassisNo,
                    PlateNo = viewModel.Jobcard.PlateNo,
                    Mileage = viewModel.Jobcard.Mileage,
                    BranchID = CurrentUser.BranchID
                };
            }       
            return View(viewModel);
        }         
    }
}