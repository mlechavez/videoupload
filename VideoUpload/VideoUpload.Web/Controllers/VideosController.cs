using Microsoft.AspNet.Identity;
using NReco.VideoConverter;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VideoUpload.Core;
using VideoUpload.Core.Entities;
using VideoUpload.Web.Common;
using VideoUpload.Web.Models;
using VideoUpload.Web.Models.Identity;
using VideoUpload.Web.Models.Videos;

namespace VideoUpload.Web.Controllers
{

    public class VideosController : AppController
    {        
        private readonly IUnitOfWork _uow;
        private readonly UserManager _mgr;

        public VideosController(IUnitOfWork unitOfWork, UserManager mgr)
        {
            _uow = unitOfWork;
            _mgr = mgr;
        }
                
        
        [AccessActionFilter(Type= "Video", Value ="CanRead")]
        public async Task<ActionResult> Posts(int page = 1)
        {            
            var viewModel = new VideoViewModel(_uow, page, 5);

            await viewModel.Initialization;

            ViewBag.Header = $"Latest Posts";            

            return View("List", viewModel);
        }
                
        public async Task<ActionResult> Search(string v)
        {
            var viewModel = new VideoViewModel(_uow, 1, 2, v);

            await viewModel.Initialization;          

            ViewBag.Header = $"Latest post found for \"{v}\"";

            return View("List", viewModel);
        }

        [ChildActionOnly]
        public PartialViewResult Sidebars()
        {
            var viewModel = new WidgetViewModel(_uow);
            
            return PartialView("_Sidebars", viewModel);
        }

        [AccessActionFilter(Type = "Video", Value = "CanCreate")]
        public ActionResult Upload()
        {
            var post = new CreatePostViewModel();            
            return View(post);            
        }

        [HttpPost]        
        [AccessActionFilter(Type = "Video", Value = "CanCreate")]
        public async Task<ActionResult> Upload(CreatePostViewModel viewModel)
        {
            var success = false;

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
                    PlateNumber = viewModel.PlateNumber,
                    Description = viewModel.Description,
                    UserID = User.Identity.GetUserId(),
                    DateUploaded = viewModel.DateUploaded,
                    BranchID = CurrentUser.BranchID
                };

                foreach (var item in viewModel.Attachments)
                {
                    if (item != null)
                    {
                        if (!contentTypeArray.Contains(item.ContentType))
                        {                            
                            ModelState.AddModelError("", "video file must be an mp4 format");

                            return Json(new { success = success, message = "Video file must be an mp4 format" });
                        }
                        countOfAttachments++;                        

                        var videoExt = Path.GetExtension(item.FileName);
                        var videoFileName = Path.GetFileName(item.FileName);

                        var videoPath = Server.MapPath("~/Uploads/Videos");
                        var thumbnailPath = Server.MapPath("~/Uploads/Thumbnails");
                     
                        //create new entity for each attachment
                        var attachment = new PostAttachment();

                        attachment.PostAttachmentID = Guid.NewGuid().ToString();
                        attachment.FileName = attachment.PostAttachmentID + videoExt;
                        attachment.MIMEType = item.ContentType;
                        attachment.FileSize = item.ContentLength;
                        attachment.FileUrl = Path.Combine(videoPath, attachment.FileName);
                        attachment.DateCreated = viewModel.DateUploaded;
                        attachment.AttachmentNo = $"Attachment {countOfAttachments.ToString()}";
                        attachment.ThumbnailFileName = attachment.PostAttachmentID + ".jpeg";
                        attachment.ThumbnailUrl = Path.Combine(thumbnailPath, attachment.ThumbnailFileName);
                        
                        var videoToSaveBeforeConvertingPath = Path.Combine(videoPath, videoFileName);                                       

                        using (var fileStream = System.IO.File.Create(videoToSaveBeforeConvertingPath))
                        {
                            var stream = item.InputStream;
                            stream.CopyTo(fileStream);
                        }
                        
                        var ffMpeg = new FFMpegConverter();
                        ffMpeg.FFMpegToolPath = videoPath; //set the tool to this path.. it's an exe file.
                        
                        var file = new FileInfo(videoToSaveBeforeConvertingPath);

                        if (file.Exists)
                        {          
                            //TODO: search for the bitrate settings                  
                            var convertSettings = new ConvertSettings
                            {
                                AudioCodec = "aac",
                                VideoCodec = "h264"                                                                                             
                            };
                            convertSettings.SetVideoFrameSize(1280, 720);
                            
                            ffMpeg.ConvertMedia(videoToSaveBeforeConvertingPath, Format.mp4, attachment.FileUrl, Format.mp4, convertSettings);
                            ffMpeg.GetVideoThumbnail(videoToSaveBeforeConvertingPath, attachment.ThumbnailUrl);

                            //Once the conversion is successful delete the original file
                            file.Delete();
                            
                            //add the attachment to post entity
                            post.Attachments.Add(attachment);                           
                        }
                    }                
                }
                var attached = post.Attachments.FirstOrDefault();
                if (attached != null)
                {
                    _uow.Posts.Add(post);
                    await _uow.SaveChangesAsync();

                    //TOD: uncomment this in production
                    //ALERT THE SERVICE MANAGER
                    //await _mgr.CustomSendEmailAsync(User.Identity.GetUserId(), "Video upload", User.Identity.Name + " has uploaded a new video", );
                    success = true;

                    return Json(new { success = success, message = "Uploaded successfully" });                   
                }
                ModelState.AddModelError("", "Attached has not been succesfully uploaded");                
            }
            return Json(new { success = success, message = "Something went wrong. Please try again" });         
        }

        [Route("{userName}/posts")]
        public async Task<ActionResult> MyPosts(int page = 1)
        {
            var viewModel = new VideoViewModel(_uow, User.Identity.GetUserId(), page, 15);

            await viewModel.Initialization;

            ViewBag.Header = "List of your videos";
            
            return View(viewModel);
        }

        [Route("{userName}/posts/{postID:int}")]
        [AccessActionFilter(Type = "Video", Value = "CanUpdate")]
        public async Task<ActionResult> Edit(string userName, int postID)
        {
            if (postID < 0) return View("_ResourceNotFound");

            //prevent typing directly the other's username 
            if (userName != User.Identity.Name) return View("_ResourceNotFound");

            var viewModel = new VideoViewModel(_uow, User.Identity.GetUserId(), postID);

            await viewModel.Initialization;

            if (viewModel.Post == null) return View("_ResourceNotFound");

            ViewBag.Header = $"Edit post for plate number: { viewModel.Post.PlateNumber }";  
                                                
            return View(viewModel.Post);
        }

        [HttpPost]
        [Route("{userName}/posts/{postID:int}")]
        [AccessActionFilter(Type = "Video", Value = "CanUpdate")]
        public async Task<ActionResult> Edit(Post viewModel)
        {
            if (!string.IsNullOrWhiteSpace(viewModel.PlateNumber) || !string.IsNullOrWhiteSpace(viewModel.Description))
            {
                var post = await _uow.Posts.GetByIdAsync(viewModel.PostID);

                post.PlateNumber = viewModel.PlateNumber;
                post.Description = viewModel.Description;
                
                await _uow.SaveChangesAsync();
                return RedirectToAction("myposts");
            }
            ModelState.AddModelError("", "All fiedls are required");
            return View(viewModel);
        }
        
        [Route("archive/{year}/{month}/{postID}/{plateNo}")]
        public async Task<ActionResult> Post(int year, int month, int postID, string plateNo)
        {            
            var post = await _uow.Posts.GetByIdAsync(postID);
                  
            if (post == null)
            {
                return View("_ResourceNotFound");
            }
            ViewBag.Header = $"Car plate no: { post.PlateNumber}";

            return View(post);
        }

        [AllowAnonymous]
        public ActionResult VideoResult(string fileName)
        {
            //return new CustomResult(fileName);
            //return new DownloadResult(fileName);         
            return new VideoStreamResult(fileName);
        }              

        [AccessActionFilter(Type = "Video", Value = "CanSend")]
        public async Task<ActionResult> Send(int postID, string sendingType)
        {
            var post = await _uow.Posts.GetByIdAsync(postID);

            if (post == null)
            {
                return View("_ResourceNotFound");
            }
            
            ViewBag.SendingType = sendingType;
            ViewBag.Header = $"Send a link to our valuable customers via: { sendingType }";
            return View(post);
        }

        [HttpPost]
        [AccessActionFilter(Type = "Video", Value = "CanSend")]
        public async Task<ActionResult> Send(Post post, FormCollection formCollection)
        {                                    
            var url = Request.Url.Scheme + "://" + Request.Url.Authority + formCollection["url"];
            var id = User.Identity.GetUserId();            
            
            var history = new History
            {
                UserID = id,
                PostID = post.PostID,
                DateSent = DateTime.UtcNow,
                Type = formCollection["sendingType"]
            };

            if (!string.IsNullOrWhiteSpace(formCollection["customerName"]))
            {
                var message = $"Dear { formCollection["customerName"] }, Please find the video for your Porsche. I will call you shortly to discuss further. Many thanks, { CurrentUser.FirstName }";
                history.Name = formCollection["customerName"];

                if (formCollection["sendingType"] == "email")
                {
                    if (string.IsNullOrWhiteSpace(formCollection["email"]))
                    {
                        ViewBag.SendingType = formCollection["sendingType"];
                        ViewBag.HasError = "has-error";
                        return View(post);
                    }
                    
                    history.Recipient = formCollection["email"];                    

                    try
                    {
                        await _mgr.CustomSendEmailAsync(id, "Your Porsche",
                            message + " " + url, formCollection["email"],
                            CurrentUser.EmailPass);
                    }
                    catch (Exception)
                    {
                        return View("Error");                        
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(formCollection["mobile"]))
                    {
                        ViewBag.SendingType = formCollection["sendingType"];
                        ViewBag.HasError = "has-error";
                        return View(post);
                    }         

                    history.Recipient = formCollection["mobile"];

                    try
                    {
                        await _mgr.OoredooSendSmsAsync(formCollection["mobile"], message + " " + url);                        
                    }
                    catch (Exception)
                    {
                        return View("Error");
                    }                    
                }
                _uow.Histories.Add(history);
                await _uow.SaveChangesAsync();
                return RedirectToAction("posts");
            }
            ViewBag.SendingType = formCollection["sendingType"];
            ViewBag.HasError = "has-error";
            return View(post);
        }        

        [AllowAnonymous]
        [Route("watch/{year}/{month}/{postID}/{plateNo}")]
        public async Task<ActionResult> Watch(int year, int month, int postID, string plateNo)
        {
            var post = await _uow.Posts.GetByIdAsync(postID);

            if (post == null)
            {
                return View("_ResourceNotFound");
            }
            ViewBag.Header = "Your Porsche";

            return View("Watch", post);            
        }

        [HttpPost]
        [AccessActionFilter(Type = "Approval", Value = "CanApproveVideo")]
        public async Task<ActionResult> Approval(bool isapproved, string postID)
        {
            var _postID = int.Parse(postID);
            var post = await _uow.Posts.GetByIdAsync(_postID);


            if (post == null)
            {
                return Json(new { success = false, message = "We could not retrieve the post. Please contact IT" });
            }
            var user = await _mgr.FindByIdAsync(post.UserID);

            if (isapproved)
            {
                post.HasApproval = isapproved;
                post.IsApproved = isapproved;
                post.DateApproved = DateTime.UtcNow;
                _uow.Posts.Update(post);
                _uow.SaveChanges();

                //await _mgr.CustomSendEmailAsync(user.Id, "Video approval", "Your video has been approved you can now send the video to customer", user.Email, "");

                return Json(new { success = true, message = "You've successfully approved the video. He/she will receive an email notification" });
            }
            post.HasApproval = true;
            post.IsApproved = isapproved;
            _uow.Posts.Update(post);
            _uow.SaveChanges();

            //await _mgr.CustomSendEmailAsync(user.Id, "Video approval", "Your video has been disapproved.", user.Email, "");

            return Json(new { success = true, message = "You've successfully disapproved the video. He/she will receive an email notification" });            
        }

        [AllowAnonymous]
        public async Task<ActionResult> VideoHasPlayed(int postID, string userName, string details)
        {
            var post = _uow.Posts.GetById(postID);
            var user = await _mgr.FindByNameAsync(userName);

            var success = false;            

            if (post != null)
            {
                if (!post.HasPlayedVideo && post.DatePlayedVideo == null && user != null)
                {
                    post.HasPlayedVideo = true;
                    post.DatePlayedVideo = DateTime.UtcNow;
                    _uow.Posts.Update(post);
                    await _uow.SaveChangesAsync();
                    success = true;
                    //alert the SA 
                    //await _mgr.CustomSendEmailAsync(user.Id, "Your video has been viewed.", "Your video has been viewed. See the details: " + details, user.Email, user.EmailPass);
                    //await _mgr.OoredooSendSmsAsync("97470064955", $"Your video with plate number {post.PlateNumber} has been played. You can now contact the customer");
                }
            }                        
            return Json(new { success = success  });
        }        
    }
}