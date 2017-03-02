using NReco.VideoConverter;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
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

        public async Task<ActionResult> Index(int? page)
        {
            var posts = await _uow.Posts.GetAllAsync();

            var viewModel = new List<PostViewModel>();
            posts.ForEach(x => 
            {
                var attachments = x.Attachments.OrderBy(y => y.AttachmentNo).ToList();

                var attachment = attachments.FirstOrDefault();
                
                if (attachment == null)
                {
                    attachments = null;
                }                                
                                
                viewModel.Add(new PostViewModel
                {
                    PostID = x.PostID,
                    Title = x.Title,
                    Description = x.Description,
                    Owner = x.Owner,
                    Attachments = attachments,
                    EditedBy = x.EditedBy,
                    DateEdited = x.DateEdited
                });
            });
            viewModel = viewModel.OrderBy(x => x.DateCreated).ToList();
            
            return View(viewModel.ToPagedList(page ?? 1, 20));
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
                        //if (!contentTypeArray.Contains(item.ContentType))
                        //{
                        //    ModelState.AddModelError("", "Please upload mp4 format for the video.");
                        //    return View(viewModel);
                        //}
                        
                        var ext = Path.GetExtension(item.FileName);
                        
                        var path = Server.MapPath("~/Uploads");

                        //create new entity for each attachment
                        var attachment = new PostAttachment();

                        attachment.PostAttachmentID = Guid.NewGuid().ToString();
                        attachment.FileName = attachment.PostAttachmentID + ext;
                        attachment.MIMEType = item.ContentType;
                        attachment.FileSize = item.ContentLength;
                        attachment.FileUrl = path + "/" + attachment.FileName;
                        attachment.DateCreated = viewModel.DateCreated;
                        attachment.AttachmentNo = $"Attachment {countOfAttachments.ToString()}";

                        //var fileUrlToConvert = Path.Combine(path, Path.GetFileName(item.FileName));
                        var fileUrlToConvert = Path.Combine(path, attachment.FileName);

                        using (var fileStream = System.IO.File.Create(fileUrlToConvert))
                        {
                            var stream = item.InputStream;
                            stream.CopyTo(fileStream);
                        }

                        //var settings = new ConvertSettings();
                        //settings.SetVideoFrameSize(640, 480);
                        //settings.AudioCodec = "aac";
                        //settings.VideoCodec = "h264";
                        //settings.VideoFrameRate = 30;

                        //var ffMpeg = new FFMpegConverter();
                        //ffMpeg.FFMpegToolPath = path; //need to have this and upload the ffmpeg.exe to this path;

                        //try
                        //{
                        //    ffMpeg.ConvertMedia(fileUrlToConvert, null, path + "/" + attachment.FileName, Format.mp4, settings);
                        //}
                        //catch (Exception ex)
                        //{
                        //    return Content(ex.Message);

                        //}

                        //ffMpeg.ConvertMedia(fileUrlToConvert,)
                        var file = new FileInfo(fileUrlToConvert);

                        if (file.Exists)
                        {
                            //add the attachment to post entity
                            post.Attachments.Add(attachment);
                            //file.Delete();
                        }                        
                    }                
                }
                var attached = post.Attachments.FirstOrDefault();
                if (attached != null)
                {
                    _uow.Posts.Add(post);
                    await _uow.SaveChangesAsync();
                    return Json(new { message = "Uploaded successfully" });
                    //return RedirectToAction("index");
                }
                ModelState.AddModelError("", "Attached has not been succesfully uploaded");                
            }
            return Json(new { message = "Something went wrong. Please try again" });
            //return View(viewModel);
        }

        public async Task<ActionResult> Edit(int postID)
        {            
            var post = await _uow.Posts.GetByIdAsync(postID);

            if (post == null)
            {
                //ViewBag.Message = "Messag goes here";
                return View("_Error");
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
                var post = await _uow.Posts.GetByIdAsync(viewModel.PostID);

                post.Title = viewModel.Title;
                post.Description = viewModel.Description;
                post.Owner = viewModel.Owner;

                await _uow.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View(viewModel);
        }
        public async Task<ActionResult> Details(int postID, string fileName)
        {            
            var post = await _uow.Posts.GetByIdAsync(postID);

            if (post == null)
            {
                return View("_Error");
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
            var file = _uow.Attachments.GetByFileName(fileName);
            
            //return new CustomResult(fileName);
            return File(file.FileUrl, file.MIMEType, file.FileName);
        }
        public ActionResult Download(string fileName)
        {
            return new DownloadResult(fileName);
        }
        public async Task<ActionResult> Send(int p, string v)
        {
            var post = await _uow.Posts.GetByIdAsync(p);

            if (post == null)
            {
                return View("_Error");
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
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(subject))
            {
                return View("_Error");
            }
            var url = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("Watch", new { p = p,  v = v });

            var mail = new MailMessage();
            mail.From = new MailAddress("kyocera.km3060@boraq-porsche.com.qa");
            mail.To.Add(email);
            mail.Subject = subject;
            mail.Body = url;
            mail.IsBodyHtml = true;

            var host = "78.100.48.220";
            var port = 25;

            using (var client = new SmtpClient(host, port))
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


            using (var fileStream = System.IO.File.Create(fileUrl))
            {
                var stream = file.InputStream;
                stream.CopyTo(fileStream);
            }

            var outputPath = Server.MapPath("~/Uploads/");
            //file.SaveAs(fileUrl);


            var settings = new ConvertSettings();
            settings.SetVideoFrameSize(640, 480);
            settings.AudioCodec = "aac";
            settings.VideoCodec = "h264";
            settings.VideoFrameRate = 30;
          
            var ffMpeg = new FFMpegConverter();

            ffMpeg.ConvertMedia(fileUrl, null, outputPath+"output.mp4", Format.mp4, settings);
                      
            return View();
        }

        public ActionResult Upload2()
        {
            var viewModel = new CreatePostViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Upload2(CreatePostViewModel viewModel)
        {
            Thread.Sleep(10000);
            try
            {
                foreach (string item in Request.Files)
                {
                    var fileContent = Request.Files[item];

                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        var stream = fileContent.InputStream;
                        var fileName = Path.GetFileName(fileContent.FileName);
                        var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }
            return Json("Upload succesfully");
        }

        public ActionResult Example()
        {
            Thread.Sleep(10000);
            string status = "Task Completed Successfully";
            return Json(status);
        }
    }
}