using Microsoft.AspNet.Identity;
using NReco.VideoConverter;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
        public async Task<ActionResult> Posts(string v, int page = 1)
        {
            IAsyncInitialization viewModel = null;

            if (string.IsNullOrWhiteSpace(v))
            {
                viewModel = new VideoViewModel(_uow, page, 5);
                ViewBag.Header = $"Latest Posts";                
            }
            else
            {
                viewModel = new VideoViewModel(_uow, page, 5, v);
                ViewBag.Header = $"Latest post found for \"{v}\"";                
            }

            await viewModel.Initialization;

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
                    Description = HttpUtility.HtmlDecode(viewModel.Description),
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
        public async Task<ActionResult> MyPosts(string userName, int page = 1)
        {
            if (User.Identity.Name != userName) return View("_ResourceNotFound");

            var viewModel = new VideoViewModel(_uow, User.Identity.GetUserId(), page, 15);

            await viewModel.Initialization;

            ViewBag.Header = "List of your videos";
            
            return View(viewModel);
        }

        [Route("{userName}/posts/{postID:int}")]
        [AccessActionFilter(Type = "Video", Value = "CanUpdate")]
        public async Task<ActionResult> Edit(string userName, int postID)
        {
            if (User.Identity.Name != userName) return View("_ResourceNotFound");

            if (postID < 0) return View("_ResourceNotFound");            

            var viewModel = new VideoViewModel(_uow, User.Identity.GetUserId(), postID);

            await viewModel.Initialization;

            if (viewModel.Post == null) return View("_ResourceNotFound");

            ViewBag.Header = $"Edit post for plate number: { viewModel.Post.PlateNumber }";

            var postedEdit = new EditPostViewModel
            {
                PostID = viewModel.Post.PostID,
                UserName = viewModel.Post.User.UserName,
                PlateNumber = viewModel.Post.PlateNumber,
                Description = viewModel.Post.Description
            };
            return View(postedEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{userName}/posts/{postID:int}")]
        [AccessActionFilter(Type = "Video", Value = "CanUpdate")]
        public async Task<ActionResult> Edit(EditPostViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var post = await _uow.Posts.GetByIdAsync(viewModel.PostID);

                post.PlateNumber = viewModel.PlateNumber.Trim();

                post.Description = viewModel.Description;

                await _uow.SaveChangesAsync();
                return RedirectToAction("myposts");
            }                   
            ModelState.AddModelError("", "All fields are required");
            return View(viewModel);
        }
        
        [Route("archive/{year:int}/{month:int}/{postID:int}/{plateNo}")]
        public async Task<ActionResult> Post(int year, int month, int postID, string plateNo)
        {                     
            var post = await _uow.Posts.GetByIdAsync(postID);
            
            if (post == null) return View("_ResourceNotFound");

            if (post.DateUploaded.Year != year || post.DateUploaded.Month != month) return View("_ResourceNotFound");
            
            ViewBag.Header = $"Car plate no: { post.PlateNumber}";

            return View(post);
        }

        [AllowAnonymous]
        public ActionResult VideoResult(string fileName)
        {
            return new CustomResult(fileName);
            //return new DownloadResult(fileName);         
            //return new VideoStreamResult(fileName);
        }              

        [AccessActionFilter(Type = "Video", Value = "CanSend")]
        public async Task<ActionResult> Send(int postID, string sendingType)
        {
            var post = await _uow.Posts.GetByIdAsync(postID);

            if (post == null) return View("_ResourceNotFound");

            ViewBag.SendingType = sendingType;
            ViewBag.Header = $"Send a link to our valuable customers via: { sendingType }";

            return View(post);
        }

        [HttpPost]
        [AccessActionFilter(Type = "Video", Value = "CanSend")]
        public async Task<ActionResult> Send(Post post, FormCollection formCollection)
        {                                                
            if (!string.IsNullOrWhiteSpace(formCollection["customerName"]))
            {
                var url = Request.Url.Scheme + "://" + Request.Url.Authority + formCollection["url"];
                var id = User.Identity.GetUserId();

                var history = new History
                {
                    UserID = id,
                    PostID = post.PostID,
                    DateSent = DateTime.UtcNow,
                    Type = formCollection["sendingType"],
                    Name = formCollection["customerName"]
                };

                var message = $"Dear { formCollection["customerName"] }, <br/><br/> Please find the video for your Porsche. I will call you shortly to discuss further.";                
                
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
                            EmailTemplate.GetTemplate(CurrentUser, message, url), formCollection["email"],
                            CurrentUser.EmailPass);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "We were unable to send your email. Please try again after a few minutes or contact your admin.";
                        _uow.AppLogs.Add(new AppLog
                        {
                            Message = ex.Message,
                            Type = ex.GetType().Name,
                            Url = Request.Url.ToString(),
                            Source = ex.InnerException.InnerException.Message,
                            UserName = User.Identity.Name,
                            LogDate = DateTime.UtcNow
                        });
                        await _uow.SaveChangesAsync();
                        return View("Error", new HandleErrorInfo(ex, "videos", "send"));                        
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
                    catch (Exception ex)
                    {
                        ViewBag.Message = "We were unable to send your sms. Please try again after a few minutes or contact your admin.";
                        _uow.AppLogs.Add(new AppLog
                        {
                            Message = ex.Message,
                            Type = ex.GetType().Name,
                            Url = Request.Url.ToString(),
                            Source = ex.InnerException.InnerException.Message,
                            UserName = User.Identity.Name,
                            LogDate = DateTime.UtcNow
                        });
                        await _uow.SaveChangesAsync();
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
        [Route("watch/{year:int}/{month:int}/{postID:int}/{plateNo}")]
        public async Task<ActionResult> Watch(int year, int month, int postID, string plateNo)
        {
            var post = await _uow.Posts.GetByIdAsync(postID);

            if (post == null || post.DateUploaded.Year != year || post.DateUploaded.Month != month)
            {
                ViewBag.Message = $"You're supposed to watch a video for your car. Please try the link again and if the problem occurs you can contact us using the below links";
                return View("_ResourceNotFound");
            }            

            ViewBag.Header = "Your Porsche";

            return View(post);            
        }

        [HttpPost]
        [AccessActionFilter(Type = "Approval", Value = "CanApproveVideo")]
        public async Task<ActionResult> Approval(bool isapproved, string postID)
        {
            var _postID = int.Parse(postID);
            var post = await _uow.Posts.GetByIdAsync(_postID);
            var message = string.Empty;

            if (post == null)
            {
                return Json(new { success = false, message = "We could not retrieve the post. Please contact your admin." });
            }

            var user = await _mgr.FindByIdAsync(post.UserID);

            if (isapproved)
            {
                post.HasApproval = isapproved;
                post.IsApproved = isapproved;
                post.DateApproved = DateTime.UtcNow;
                message = "You've successfully approved the video. He/she will receive an email notification";
            }
            else
            {
                //set  to true even not approved
                post.HasApproval = true;
                message = "You've successfully disapproved the video. He/she will receive an email notification";
            }
                                              
            _uow.Posts.Update(post);
            _uow.SaveChanges();

            try
            {
                await _mgr.CustomSendEmailAsync(user.Id,
                    "Video approval", "Your video has been approved you can now send the video to customer",
                    user.Email, user.EmailPass);
            }
            catch (Exception ex)
            {
                _uow.AppLogs.Add(new AppLog
                {
                    Message = ex.Message,
                    Type = ex.GetType().Name,
                    Url = Request.Url.ToString(),
                    Source = ex.InnerException.InnerException.Message,
                    UserName = User.Identity.Name,
                    LogDate = DateTime.UtcNow
                });
                await _uow.SaveChangesAsync();

                return Json(new
                {
                    success = false,                    
                    message = "You've successfully approved/disapproved the video but it seems our email is currently not available and we're not able to send email to the service advisor."
                });
            }

            return Json(new { success = true, message = message });            
        }

        [AllowAnonymous]
        public async Task<ActionResult> VideoHasPlayed(int postID, string userName, string details)
        {
            var post = _uow.Posts.GetById(postID);
            var user = await _mgr.FindByNameAsync(userName);

            var success = false;

            if (post == null) return Json(new { success = success });
            
            if (!post.HasPlayedVideo && post.DatePlayedVideo == null && user != null)
            {
                post.HasPlayedVideo = true;
                post.DatePlayedVideo = DateTime.UtcNow;
                _uow.Posts.Update(post);
                await _uow.SaveChangesAsync();
                success = true;

                //alert the SA 
                try
                {                    
                    await _mgr.CustomSendEmailAsync(user.Id, "Your posted video.", 
                         EmailTemplate.GetTemplate(CurrentUser, "Your video has been viewed. See the details: ", details),
                        user.Email, user.EmailPass);
                }
                catch (Exception eExcp)
                {
                    var eAppLog = new AppLog
                    {
                        Message = eExcp.Message,
                        Type = eExcp.GetType().Name,
                        Url = Request.Url.ToString(),
                        Source = eExcp.InnerException.InnerException.Message,
                        UserName = User.Identity.Name,
                        LogDate = DateTime.UtcNow
                    };
                    _uow.AppLogs.Add(eAppLog);

                    //Try to send via text when email is not available
                    try
                    {
                        await _mgr.OoredooSendSmsAsync(user.MobileNumber, $"Your video with plate number {post.PlateNumber} has been played. You can now contact the customer");
                        
                        //save the email log exception
                        _uow.SaveChanges();
                    }
                    catch (Exception sExcp)
                    {
                        var sAppLog = new AppLog
                        {
                            Message = sExcp.Message,
                            Type = sExcp.GetType().Name,
                            Url = Request.Url.ToString(),
                            Source = sExcp.InnerException.InnerException.Message,
                            UserName = User.Identity.Name,
                            LogDate = DateTime.UtcNow
                        };

                        await _uow.SaveChangesAsync();
                    }

                }
            }
            return Json(new { success = success });
        }        
    }
}