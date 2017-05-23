using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VideoUpload.Core;
using VideoUpload.Core.Entities;
using VideoUpload.Web.Common;
using VideoUpload.Web.Models.HealthChecks;

namespace VideoUpload.Web.Controllers
{
    [RoutePrefix("hc")]
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
        
        [Route("upload")]
        public ActionResult Upload()
        {           
            return View();
        }

        [HttpPost]
        [Route("upload")]
        public ActionResult Upload(HttpPostedFileBase csv)
        {
            var jobcardNo = string.Empty;
            
            if (csv != null && csv.ContentLength > 0 && csv.ContentType == "application/vnd.ms-excel")
            {                
                jobcardNo = UploadCsvToDatabase(csv.InputStream);

                if (!string.IsNullOrEmpty(jobcardNo))
                {
                    return RedirectToAction("details", new { jobcardNo = jobcardNo });
                }
            }
            else
            {
                ViewBag.UploadStatus = "Please double check the format of the file. Make sure it is in CSV format";
            }            
            return View();
        }        

        private string UploadCsvToDatabase(Stream inputStream)
        {
            //var isSuccess = false;
            var line = string.Empty;
            string[] strArray;
            var jobcardNo = string.Empty;
            var customerName = string.Empty;
            var chassisNo = string.Empty;
            var plateNo = string.Empty;
            var mileage = string.Empty;
            var hcCode = string.Empty;
            var status = string.Empty;
            var comments = string.Empty;

            //We should not seperate the comma in a sentence
            Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            StreamReader sr = new StreamReader(inputStream);

            //read the first row which the column header
            line = sr.ReadLine();
            strArray = r.Split(line);

            //_uow.Jobcards.GetById()
            var jobcard = new Jobcard();             

            while ((line = sr.ReadLine()) != null)
            {                
                strArray = r.Split(line);

                jobcardNo = !string.IsNullOrWhiteSpace(strArray[0]) ? strArray[0].ToString().Replace($"\"","") : string.Empty;
                customerName = !string.IsNullOrWhiteSpace(strArray[1]) ? strArray[1].ToString().Replace($"\"", null) : string.Empty;
                chassisNo = !string.IsNullOrWhiteSpace(strArray[2]) ? strArray[2].ToString().Replace($"\"", null) : string.Empty;
                plateNo = !string.IsNullOrWhiteSpace(strArray[3]) ? strArray[3].ToString().Replace($"\"", null) : string.Empty;
                mileage = !string.IsNullOrWhiteSpace(strArray[4]) ? strArray[4].ToString().Replace($"\"", null) : string.Empty;
                hcCode = !string.IsNullOrWhiteSpace(strArray[5]) ? strArray[5].ToString().Replace($"\"", null) : string.Empty;
                status = !string.IsNullOrWhiteSpace(strArray[6]) ? strArray[6].ToString().Replace($"\"", null) : string.Empty;                
                comments = !string.IsNullOrWhiteSpace(strArray[7]) ? strArray[7].ToString().Replace($"\"", null) : string.Empty;

                if (jobcard.JobcardNo == null)
                {
                    jobcard.JobcardNo = jobcardNo;                    
                    jobcard.CustomerName = customerName;
                    jobcard.ChassisNo = chassisNo;
                    jobcard.PlateNo = plateNo;
                    jobcard.Mileage = mileage;
                    jobcard.BranchID = CurrentUser.BranchID;
                }

                jobcard.HealthCheckDetails.Add(new HealthCheckDetails
                {
                    JobcardNo = jobcardNo,
                    HcCode = hcCode,
                    Status = status,
                    Comments = comments
                });                
            }            
            _uow.Jobcards.Add(jobcard);

            _uow.SaveChanges();
            sr.Dispose();
            //isSuccess = true;
            return jobcardNo;
        }

        public ActionResult ExportVhcToCsv(string jobcardNo)
        {
            var jobcard = _uow.Jobcards.GetById(jobcardNo);

            StringBuilder sb = new StringBuilder();

            //header column
            sb.Append("Jobcard no").Append(",")
              .Append("Customer name").Append(",")
              .Append("Chassis no").Append(",")
              .Append("Plate no").Append(",")
              .Append("Mileage").Append(",")
              .Append("Hc Code").Append(",")
              .Append("Status").Append(",")
              .Append("Comments").Append(",")
              .AppendLine();

            foreach (var item in jobcard.HealthCheckDetails)
            {
                sb.Append(jobcard.JobcardNo).Append(",")
                  .Append(jobcard.CustomerName).Append(",")
                  .Append(jobcard.ChassisNo).Append(",")
                  .Append(jobcard.PlateNo).Append(",")
                  .Append(jobcard.Mileage).Append(",")
                  .Append(item.HcCode).Append(",")
                  .Append(item.Status).Append(",")
                  .Append(item.Comments).Append(",")
                  .AppendLine();
            }
            var data = Encoding.UTF8.GetBytes(sb.ToString());
            return File(data, "text/csv", "vhc.csv");
        }

        [Route("new")]
        public ActionResult New()
        {
            var viewModel = new HcViewModel(_uow);            
            return View(viewModel);
        }
        [HttpPost]
        [Route("new")]
        public async Task<ActionResult> New(HcViewModel viewModel)
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
                _uow.Jobcards.Add(jobcard);
                
                //add the jobcard foreach before saiving to db
                viewModel.HealthCheckDetails.ToList().ForEach(x =>
                {
                    x.JobcardNo = jobcard.JobcardNo;
                });

                _uow.HealthCheckDetails.AddRange(viewModel.HealthCheckDetails.ToList());                
                await _uow.SaveChangesAsync();
                return RedirectToAction("index", "videos");
            }

            SetJobcardValidationErrorMessages(ModelState);
                        
            viewModel.GroupedHealthChecks = _uow.HealthChecks.GetAllByHcGroup();   
            return View(viewModel);
        }

        [AllowAnonymous]
        [Route("{jobcardNo}")]
        public ActionResult Details(string jobcardNo)
        {
            var viewModel = new HcViewModel(_uow, jobcardNo);

            return View(viewModel);
        }

        private void SetJobcardValidationErrorMessages(ModelStateDictionary modelState)
        {
            string hasError = "has-error";
            string placeHolder = "This field is required";
            IEnumerable<string> keys = modelState.Where(x => x.Value.Errors.Any()).Select(x => x.Key);

            foreach (var key in keys)
            {
                switch (key)
                {
                    case "Jobcard.CustomerName":
                        ViewBag.CustomerNameError = hasError;
                        ViewBag.CustomerNamePlaceholder = placeHolder;
                        break;
                    case "Jobcard.JobcardNo":
                        ViewBag.JobcardNoError = hasError;
                        ViewBag.JobcardNoPlaceholder = placeHolder;
                        break;
                    case "Jobcard.ChassisNo":
                        ViewBag.ChassisNoError = hasError;
                        ViewBag.ChassisNoPlaceholder = placeHolder;
                        break;
                    case "Jobcard.PlateNo":
                        ViewBag.PlateNoError = hasError;
                        ViewBag.PlateNoPlaceholder = placeHolder;
                        break;
                    case "Jobcard.Mileage":
                        ViewBag.MileageError = hasError;
                        ViewBag.MileagePlaceholder = placeHolder;
                        break;
                    default:
                        break;
                }
            }
            
        }
    }
}