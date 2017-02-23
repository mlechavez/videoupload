using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using VideoUpload.Core.Entities;
using VideoUpload.EF;
using VideoUpload.Web.Models;

namespace VideoUpload.Web.Controllers
{
    public class VideosController : Controller
    {
        private readonly UnitOfWork _uow;
        public VideosController(UnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
        public ActionResult Index()
        {
            var videos = _uow.Videos.GetAll();
            var viewModel = new List<VideoViewModel>();
            videos.ForEach(x => 
            {
                viewModel.Add(new VideoViewModel
                {
                    VideoID = x.VideoID,
                    Title = x.Title,
                    Description = x.Description
                });
            });
            return View(viewModel);
        }

        public ActionResult Upload()
        {
            var video = new VideoViewModel();
            return View(video);
        }

        [HttpPost]
        public async Task<ActionResult> Upload(VideoViewModel viewModel)
        {
            
            if (ModelState.IsValid)
            {
                var countOfAttachments = 0;

                var contentTypeArray = new string[] 
                {
                    "video/mp4",
                    "video/avi",
                    "application/x-mpegURL",
                    "video/MP2T",
                    "video/3gpp",
                    "video/quicktime",
                    "video/x-msvideo",
                    "video/x-ms-wmv"
                };

                foreach (var item in viewModel.Videos)
                {
                    if (item != null)
                    {
                        //var fileName = Path.GetFileName(item.FileName);
                        var ext = Path.GetExtension(item.FileName);
                        viewModel.VideoFileName = viewModel.VideoID + ext;
                                                 
                        var fileUrl = Path.Combine(Server.MapPath("~/Uploads"), viewModel.VideoFileName);

                        item.SaveAs(fileUrl);

                        countOfAttachments++;
                    }
                    else
                    {
                        countOfAttachments--;
                    }
                }


                var video = new Video
                {
                    VideoID = viewModel.VideoID,
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    VideoFileName = viewModel.VideoFileName
                };
                _uow.Videos.Add(video);
                await _uow.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View(viewModel);
        }

        public ActionResult Edit(string videoID)
        {            
            var video = _uow.Videos.GetById(videoID);

            if (video == null)
            {
                return HttpNotFound();
            }
            
            var viewModel = new VideoViewModel
            {
                VideoID = video.VideoID,
                Title = video.Title,
                Description = video.Description,
                VideoFileName = video.VideoFileName
            };
                        
            return View(viewModel);
        }

        public ActionResult Details(string videoID)
        {
            
            var video = _uow.Videos.GetById(videoID);

            if (video == null)
            {
                return HttpNotFound();
            }

            var viewModel = new VideoViewModel
            {
                VideoID = video.VideoID,
                Title = video.Title,
                Description = video.Description,
                VideoFileName = video.VideoFileName
            };

            return View(viewModel);
        }

        public ActionResult VideoResult(string fileName)
        {
            return new CustomResult(fileName);
        }

        public ActionResult Send(string v)
        {
            var video = _uow.Videos.GetById(v);

            if (video == null)
            {
                return HttpNotFound();
            }

            var viewModel = new VideoViewModel
            {
                VideoID = video.VideoID,
                Title = video.Title,
                Description = video.Description,
                VideoFileName = video.VideoFileName
            };

            return View(viewModel);            
        }

        [HttpPost]
        public  ActionResult Send(string email, string subject, string v)
        {
            var url = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("Watch", new { v = v });

            var mail = new MailMessage();
            mail.From = new MailAddress("kyocera.km3060@boraq-porsche.com.qa");
            mail.To.Add(email);
            mail.Body = url;
            mail.IsBodyHtml = true;

            using (var client = new SmtpClient("192.168.5.10", 25))
            {
                client.Credentials = new System.Net.NetworkCredential("kyocera.km3060@boraq-porsche.com.qa", "kyocera123");
                client.Send(mail);
            }

            return RedirectToAction("index");
        }

        public ActionResult Watch(string v)
        {
            var video = _uow.Videos.GetById(v);

            if (video == null)
            {
                return HttpNotFound();
            }

            var viewModel = new VideoViewModel
            {
                VideoID = video.VideoID,
                Title = video.Title,
                Description = video.Description,
                VideoFileName = video.VideoFileName
            };

            return View(viewModel);
        }
    }
}