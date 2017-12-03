using Microsoft.AspNet.Identity;
using NReco.VideoConverter;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
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

        [AccessActionFilter(Type = "Video", Value = "CanRead")]
        [Route("")]
        public async Task<ActionResult> Posts(string s, int? page)
        {
            var viewModel =
                await VideoViewModel.CreateAsync(
                uow: _uow,
                pageIndex: page ?? 1,
                pageSize: 3,
                filterType: string.Empty,
                param: string.Empty);

            ViewBag.Header = "Latest Posts";

            return View("List", viewModel);
        }
        [Route("search")]
        public async Task<ActionResult> Search(string s, int? page)
        {
            var viewModel =
                await VideoViewModel.CreateAsync(
                uow: _uow,
                pageIndex: page ?? 1,
                pageSize: 3,
                filterType: "search",
                param: s);

            ViewBag.Header = $"List of posts found for \"{s}\"";

            return View("List", viewModel);
        }

        [ChildActionOnly]
        public PartialViewResult Sidebars()
        {
            //MVC 5 does not support asynchronous in partial view. Fetch the data synchronously 
            ViewBag.ApprovedVideos = VideoWidgetViewModel.Create(_uow, 1, 10, "approved", string.Empty);
            ViewBag.HasPlayedVideos = VideoWidgetViewModel.Create(_uow, 1, 10, "hasplayed", string.Empty);

            return PartialView("_Sidebars");
        }

        [AccessActionFilter(Type = "Video", Value = "CanCreate")]
        public ActionResult Upload()
        {
            return View();
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

                //create new post entity
                var post = new Post
                {
                    PlateNumber = viewModel.PlateNumber,
                    Description = HttpUtility.HtmlDecode(viewModel.Description),
                    UserID = User.Identity.GetUserId(),
                    DateUploaded = viewModel.DateUploaded,
                    BranchID = CurrentUser.BranchID
                };

                // I removed the foreach loop
                // and use this instead since the posted file will always be one
                // and for performance reason
                var item = viewModel.Attachments.FirstOrDefault();

                //check if the item is null
                //or else throw an error
                if (item != null)
                {
                    //check if the content type is an MP4                        
                    if (!contentTypeArray.Contains(item.ContentType))
                    {
                        ModelState.AddModelError("", "video file must be an mp4 format");

                        return Json(new { success = success, message = "Video file must be an mp4 format" });
                    }

                    //increment the count of attachment                    
                    countOfAttachments++;

                    //get the fileName extension 
                    var videoExt = Path.GetExtension(item.FileName);
                    //get the fileName
                    var videoFileName = Path.GetFileName(item.FileName);

                    //set the video path
                    var videoPath = Server.MapPath("~/Uploads/Videos");
                    //set the thumbnail path
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

                    //concatenate the path and the filename
                    var videoToSaveBeforeConvertingPath = Path.Combine(videoPath, videoFileName);

                    //save the video
                    using (var fileStream = System.IO.File.Create(videoToSaveBeforeConvertingPath))
                    {
                        var stream = item.InputStream;
                        stream.CopyTo(fileStream);
                    }

                    //for conversion
                    var ffMpeg = new FFMpegConverter();

                    
                    //Set the path of the exe of FFMpeg
                    ffMpeg.FFMpegToolPath = videoPath;

                    //create a file instance 
                    var file = new FileInfo(videoToSaveBeforeConvertingPath);

                    //check if the file exists after saving the video                    
                    if (file.Exists)
                    {
                        //codec for mp4
                        var convertSettings = new ConvertSettings
                        {
                            AudioCodec = "aac",
                            VideoCodec = "h264"
                        };
                        //set the resolution
                        convertSettings.SetVideoFrameSize(1280, 720);

                        //convert the saved file
                        //attachment.FileUrl is the new output filename 
                        ffMpeg.ConvertMedia(videoToSaveBeforeConvertingPath, Format.mp4, attachment.FileUrl, Format.mp4, convertSettings);

                        //get the thumbnail of the video for 
                        ffMpeg.GetVideoThumbnail(attachment.FileUrl, attachment.ThumbnailUrl);

                        //Once the conversion is successful delete the original file
                        file.Delete();

                        //add the attachment to post entity
                        post.Attachments.Add(attachment);
                    }
                }


                //find the first attachment                
                var attached = post.Attachments.FirstOrDefault();

                //if the attachment is not null save it else throw an error
                if (attached != null)
                {
                    //add the post entity and save
                    _uow.Posts.Add(post);
                    await _uow.SaveChangesAsync();


                    //fetch the end-users who have approval access                    
                    var claims = await _uow.UserClaims.GetAllByClaimTypeAndValueAsync("Approval", "CanApproveVideo");

                    if (claims.Count > 0)
                    {
                        StringBuilder recipients = new StringBuilder();

                        //loop through and create a semicolon separated values
                        //to be used in sending email notification to the supervisors
                        claims.ForEach(claim =>
                        {
                            recipients.Append(claim.User.Email)
                              .Append(";");
                        });

                        //get the url of the posted video 
                        //to be included in the email
                        var url = Request.Url.Scheme + "://" + Request.Url.Authority +
                            Url.Action("post", new
                            {
                                year = post.DateUploaded.Year,
                                month = post.DateUploaded.Month,
                                postID = post.PostID,
                                plateNo = post.PlateNumber
                            });

                        //send email
                        try
                        {
                            await _mgr.CustomSendEmailAsync(
                            User.Identity.GetUserId(),
                            "New posted video",
                            EmailTemplate.GetTemplate(
                                CurrentUser,
                                "Supervisors",
                                "I have posted a new video. Please see the details for approval.",
                                url),
                            recipients.ToString());
                        }
                        catch (SmtpException ex)
                        {
                            _uow.AppLogs.Add(new AppLog
                            {
                                Message = ex.Message,
                                Type = ex.GetType().Name,
                                Url = Request.Url.ToString(),
                                Source = (ex.InnerException != null) ? ex.InnerException.Message : string.Empty,
                                UserName = User.Identity.Name,
                                LogDate = DateTime.UtcNow
                            });
                            await _uow.SaveChangesAsync();

                            success = true;

                            return Json(new { success = success, message = "Uploaded successfully" });
                        }
                    }
                    success = true;

                    return Json(new { success = success, message = "Uploaded successfully" });
                }
                ModelState.AddModelError("", "Attached has not been succesfully uploaded");
            }
            return Json(new { success = success, message = "Something went wrong. Please try again" });
        }

        [Route("{userName}/posts")]
        public async Task<ActionResult> MyPosts(string userName, int? page)
        {
            //this checks if the user is trying to change the name in the url
            if (User.Identity.Name != userName) return View("_ResourceNotFound");

            var viewModel =
                await VideoViewModel.CreateAsync(
                    uow: _uow,
                    pageIndex: page ?? 1,
                    pageSize: 10,
                    filterType: "user",
                    param: User.Identity.GetUserId());

            ViewBag.Header = "List of your posts";

            return View(viewModel);
        }

        [Route("{userName}/posts/{postID:int}")]
        [AccessActionFilter(Type = "Video", Value = "CanUpdate")]
        public async Task<ActionResult> Edit(string userName, int postID)
        {
            //this checks if the user is trying to change the name in the url
            if (User.Identity.Name != userName) return View("_ResourceNotFound");

            if (postID < 0) return View("_ResourceNotFound");

            var post = await _uow.Posts.GetByUserIDAndPostIDAsync(User.Identity.GetUserId(), postID);

            if (post == null) return View("_ResourceNotFound");

            ViewBag.Header = $"Edit post for plate number: { post.PlateNumber }";

            var postedEdit = new EditPostViewModel
            {
                PostID = post.PostID,
                UserID = post.UserID,
                PlateNumber = post.PlateNumber,
                Description = post.Description
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
                var post = await _uow.Posts.GetByUserIDAndPostIDAsync(viewModel.UserID, viewModel.PostID);

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
            var post = await _uow.Posts.GetByDateUploadedAndPostIDAsync(year, month, postID);

            if (post == null) return View("_ResourceNotFound");

            if (post.DateUploaded.Year != year || post.DateUploaded.Month != month) return View("_ResourceNotFound");

            ViewBag.Header = $"Car plate no: { post.PlateNumber}";

            return View(post);
        }

        [AllowAnonymous]
        public ActionResult VideoResult(string fileName)
        {
            return new CustomResult(fileName);
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
                var url = $"{Request.Url.Scheme}://{Request.Url.Authority}{formCollection["url"]}";

                //send request to shorten the URL
                //var response = await GoogleUrlShortener.GetUrlShortenerAsync(url);

                //if (!response.IsSuccessStatusCode)
                //{
                //    return View(post);
                //}

                //var content = await response.Content.ReadAsStringAsync();
                //var shortenedUrlInfo = JsonConvert.DeserializeObject<ShortenedUrlInfo>(content);
                

                var history = new History
                {
                    UserID = User.Identity.GetUserId(),
                    PostID = post.PostID,
                    DateSent = DateTime.UtcNow,
                    Type = formCollection["sendingType"],
                    Name = formCollection["customerName"]
                };

                var recipient = formCollection["customerName"];
                var message = "Please find the video for your Porsche. I will call you shortly to discuss further.";

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
                        await _mgr.CustomSendEmailAsync(
                            User.Identity.GetUserId(),
                            "Your Porsche",
                            EmailTemplate.GetTemplate(
                                CurrentUser,
                                recipient,
                                message,
                                url),
                            formCollection["email"].Trim().ToString());
                    }
                    catch (SmtpException ex)
                    {
                        _uow.AppLogs.Add(new AppLog
                        {
                            Message = ex.Message,
                            Type = ex.GetType().Name,
                            Url = Request.Url.ToString(),
                            Source = (ex.InnerException != null) ? ex.InnerException.Message : string.Empty,
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
                        StringBuilder msg = new StringBuilder();
                        msg.AppendLine($"Dear { recipient },")
                           .AppendLine("Please find the video for your Porsche.")
                           .AppendLine(url)
                           .AppendLine("I will call you shortly to discuss further.");

                        await _mgr.OoredooSendSmsAsync(formCollection["mobile"], msg.ToString());
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "We were unable to send your sms. Please try again after a few minutes or contact your admin.";
                        _uow.AppLogs.Add(new AppLog
                        {
                            Message = ex.Message,
                            Type = ex.GetType().Name,
                            Url = Request.Url.ToString(),
                            Source = (ex.InnerException != null) ? ex.InnerException.Message : string.Empty,
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
            if (User.Identity.IsAuthenticated)
            {
                return View("_ResourceNotFound");
            }
            var post = await _uow.Posts.GetByDateUploadedAndPostIDAsync(year, month, postID);

            if (post == null)
            {
                ViewBag.Message = $"You're supposed to watch a video for your car. Please try the link again and if the problem occurs you can contact us using the below links";
                return View("_ErrorWatch");
            }

            ViewBag.Header = "Your Porsche";
            return View(post);
        }

        [HttpPost]
        [AccessActionFilter(Type = "Approval", Value = "CanApproveVideo")]
        public async Task<ActionResult> Approval(bool isapproved, string postID, string details)
        {
            var _postID = int.Parse(postID);
            var post = await _uow.Posts.GetByIdAsync(_postID);
            var message = string.Empty;
            var status = string.Empty;

            if (post == null)
            {
                return Json(new { success = false, message = "We could not retrieve the post. Please contact your admin." });
            }

            if (isapproved)
            {
                post.HasApproval = isapproved;
                post.IsApproved = isapproved;
                post.DateApproved = DateTime.UtcNow;
                message = "You've successfully approved the video. He/she will receive an email notification";
                status = "approved";
            }
            else
            {
                //set  to true even not approved
                post.HasApproval = true;
                message = "You've successfully disapproved the video. He/she will receive an email notification";
                status = "disapproved";
            }
            post.Approver = User.Identity.Name;
            _uow.Posts.Update(post);
            _uow.SaveChanges();

            try
            {
                var user = await _mgr.FindByIdAsync(post.UserID);

                await _mgr.CustomSendEmailAsync(
                    User.Identity.GetUserId(),
                    "Video approval",
                    EmailTemplate.GetTemplate(
                        CurrentUser,
                        user.FirstName,
                        $"Your video has been { status }.", details),
                        user.Email);
            }
            catch (SmtpException ex)
            {
                _uow.AppLogs.Add(new AppLog
                {
                    Message = ex.Message,
                    Type = ex.GetType().Name,
                    Url = Request.Url.ToString(),
                    Source = (ex.InnerException != null) ? ex.InnerException.Message : string.Empty,
                    UserName = User.Identity.Name,
                    LogDate = DateTime.UtcNow
                });
                await _uow.SaveChangesAsync();

                return Json(new
                {
                    success = false,
                    message = "You've successfully approved/disapproved the video but could not send an email to the service advisor."
                });
            }

            return Json(new { success = true, message = message });
        }

        [AllowAnonymous]
        public async Task<ActionResult> VideoHasPlayed(int postID, string userName, string details)
        {
            var post = _uow.Posts.GetById(postID);

            var admin = await _mgr.FindByNameAsync("admin"); //to get the alboraq.app@boraq-porsche email

            var success = false;

            if (post == null) return Json(new { success = success });

            if (!post.HasPlayedVideo && post.DatePlayedVideo == null && admin != null)
            {
                post.HasPlayedVideo = true;
                post.DatePlayedVideo = DateTime.UtcNow;
                _uow.Posts.Update(post);
                await _uow.SaveChangesAsync();
                success = true;

                //alert the SA 
                try
                {
                    var owner = await _mgr.FindByIdAsync(post.UserID); //get the owner of the posted video

                    await _mgr.CustomSendEmailAsync(
                        admin.Id,
                        "Your posted video.",
                         EmailTemplate.GetTemplate(
                             admin,
                             owner.FirstName,
                             $"Your video with plate number { post.PlateNumber } has been played. You can now contact the customer. For more details: ",
                             details),
                        owner.Email);
                }
                catch (SmtpException eExcp)
                {
                    var eAppLog = new AppLog
                    {
                        Message = eExcp.Message,
                        Type = eExcp.GetType().Name,
                        Url = Request.Url.ToString(),
                        Source = (eExcp.InnerException != null) ? eExcp.InnerException.Message : string.Empty,
                        UserName = admin.UserName,
                        LogDate = DateTime.UtcNow
                    };
                    _uow.AppLogs.Add(eAppLog);

                    //Try to send via text when email is not available
                    try
                    {
                        await _mgr.OoredooSendSmsAsync(admin.MobileNumber, $"Your video with plate number {post.PlateNumber} has been played. You can now contact the customer");

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
                            Source = (sExcp.InnerException != null) ? sExcp.InnerException.Message : string.Empty,
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