using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
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
            var posts = _uow.Posts.GetAll();
            var viewModel = new List<PostViewModel>();
            posts.ForEach(x => 
            {
                var attachments = x.Attachments.OrderBy(y => y.AttachmentNo).ToList();
                                
                viewModel.Add(new PostViewModel
                {
                    PostID = x.PostID,
                    Title = x.Title,
                    Description = x.Description,
                    Owner = x.Owner,
                    Attachments = attachments
                });
            });
            
            return View(viewModel);
        }

        public ActionResult Upload()
        {
            var post = new CreatePostViewModel();
            return View(post);
        }

        [HttpPost]
        public async Task<ActionResult> Upload(CreatePostViewModel viewModel)
        {           
            if (ModelState.IsValid)
            {
                var countOfAttachments = 0;

                var contentTypeArray = new string[] 
                {
                    "video/mp4"
                    //"video/avi",
                    //"application/x-mpegURL",
                    //"video/MP2T",
                    //"video/3gpp",
                    //"video/quicktime",
                    //"video/x-msvideo",
                    //"video/x-ms-wmv"
                };

                var post = new Post
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    Owner = viewModel.Owner,
                    DateCreated = viewModel.DateCreated
                };

                foreach (var item in viewModel.Attachments)
                {
                    if (item != null)
                    {
                        countOfAttachments++;
                        if (!contentTypeArray.Contains(item.ContentType))
                        {
                            ModelState.AddModelError("", "Please upload mp4 format for the video.");
                            return View(viewModel);
                        }
                        //var fileName = Path.GetFileName(item.FileName);
                        var ext = Path.GetExtension(item.FileName);

                        //create new entity for each attachment
                        var attachment = new PostAttachment();

                        attachment.PostAttachmentID = Guid.NewGuid().ToString();
                        attachment.FileName = attachment.PostAttachmentID + ext;
                        attachment.MIMEType = item.ContentType;
                        attachment.FileSize = item.ContentLength;
                        attachment.DateCreated = viewModel.DateCreated;
                        attachment.AttachmentNo = $"Attachment {countOfAttachments.ToString()}";

                        var fileUrl = Path.Combine(Server.MapPath("~/Uploads"), attachment.FileName);

                        item.SaveAs(fileUrl);

                        //add the attachment to post entity
                        post.Attachments.Add(attachment);                                          
                    }                
                }               
                _uow.Posts.Add(post);
                await _uow.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View(viewModel);
        }

        public ActionResult Edit(int postID)
        {            
            var post = _uow.Posts.GetById(postID);

            if (post == null)
            {
                return HttpNotFound();
            }
            
            var viewModel = new PostViewModel
            {
                PostID = post.PostID,
                Title = post.Title,
                Description = post.Description,
                Owner = post.Owner                              
            };
                        
            return View(viewModel);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(PostViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var post = _uow.Posts.GetById(viewModel.PostID);

                post.Title = viewModel.Title;
                post.Description = viewModel.Description;
                post.Owner = viewModel.Owner;

                await _uow.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View(viewModel);
        }
        public ActionResult Details(int postID, string fileName)
        {            
            var post = _uow.Posts.GetById(postID);

            if (post == null)
            {
                return HttpNotFound();
            }

            var attachment = post.Attachments.FirstOrDefault(x => x.FileName == fileName);
            
            ViewBag.FileName = attachment.FileName;
            ViewBag.MIMEType = attachment.MIMEType;
            ViewBag.AttachmentNo = attachment.AttachmentNo;

            //for some reason i need to orderby the attachment
            var attachments = post.Attachments.OrderBy(x => x.AttachmentNo).ToList();

            var viewModel = new PostViewModel
            {
                PostID = post.PostID,
                Title = post.Title,
                Description = post.Description,
                Owner = post.Owner,
                Attachments = attachments
            };            
            return View(viewModel);
        }
        public ActionResult VideoResult(string fileName)
        {
            return new CustomResult(fileName);
        }
        public ActionResult Download(string fileName)
        {
            return new DownloadResult(fileName);
        }
        public ActionResult Send(int p, string v)
        {
            var post = _uow.Posts.GetById(p);

            if (post == null)
            {
                return HttpNotFound();
            }

            var attachment = post.Attachments.FirstOrDefault(x => x.FileName == v);

            ViewBag.FileName = attachment.FileName;
            ViewBag.MIMEType = attachment.MIMEType;
            ViewBag.AttachmentNo = attachment.AttachmentNo;

            //for some reason i need to orderby the attachment
            var attachments = post.Attachments.OrderBy(x => x.AttachmentNo).ToList();

            var viewModel = new PostViewModel
            {
                PostID = post.PostID,
                Title = post.Title,
                Description = post.Description,
                Owner = post.Owner,
                Attachments = attachments
            };
            return View(viewModel);
        }
        [HttpPost]
        public  ActionResult Send(string email, string subject,int p, string v)
        {
            var url = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("Watch", new { p = p,  v = v });

            var mail = new MailMessage();
            mail.From = new MailAddress("kyocera.km3060@boraq-porsche.com.qa");
            mail.To.Add(email);
            mail.Subject = subject;
            mail.Body = url;
            mail.IsBodyHtml = true;

            using (var client = new SmtpClient("78.100.48.220", 25))
            {
                client.Credentials = new System.Net.NetworkCredential("kyocera.km3060@boraq-porsche.com.qa", "kyocera123");
                client.EnableSsl = false;
                client.Send(mail);
            }
            return RedirectToAction("index");
        }

        public ActionResult Watch(int p, string v)
        {
            var post = _uow.Posts.GetById(p);

            if (post == null)
            {
                return HttpNotFound();
            }

            var attachment = post.Attachments.FirstOrDefault(x => x.FileName == v);

            ViewBag.FileName = attachment.FileName;
            ViewBag.MIMEType = attachment.MIMEType;
            ViewBag.AttachmentNo = attachment.AttachmentNo;

            //for some reason i need to orderby the attachment
            var attachments = post.Attachments.OrderBy(x => x.AttachmentNo).ToList();

            var viewModel = new PostViewModel
            {
                PostID = post.PostID,
                Title = post.Title,
                Description = post.Description,
                Owner = post.Owner,
                Attachments = attachments
            };
            return View(viewModel);
        }

        public ActionResult ConvertAndUpload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ConvertAndUpload(HttpPostedFileBase file)
        {
            
            var filename = Path.GetFileName(file.FileName);            
            var fileUrl = Path.Combine(Server.MapPath("~/Uploads"), filename);

            var outputPath = Server.MapPath("~/Uploads/");
            //file.SaveAs(fileUrl);


            var settings = new ConvertSettings();
            settings.SetVideoFrameSize(640, 480);
            settings.AudioCodec = "aac";
            settings.VideoCodec = "h264";
            settings.VideoFrameRate = 60;

            //var sr = new StreamReader(file.InputStream);
            //var result = sr.ReadToEnd();
            //sr.Close();
            var ffMpeg = new FFMpegConverter();

            var testResult = ffMpeg.ConvertLiveMedia(file.InputStream,null, outputPath + "output.mp4", Format.mp4, settings);
            testResult.Start();
            
            //var path = Path.GetFullPath(fileUrl);

            //var outputPath = Server.MapPath("~/Uploads/");                                          

            return View();
        }
    }
}