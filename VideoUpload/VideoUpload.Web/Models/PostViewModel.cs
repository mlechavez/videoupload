using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VideoUpload.Core;
using VideoUpload.Core.Entities;

namespace VideoUpload.Web.Models
{
    public class PostViewModel
    {
        private readonly IUnitOfWork _uow;
        public PostViewModel()
        {
            Attachments = new HashSet<PostAttachment>();
        }

        public PostViewModel(IUnitOfWork uow)
        {
            _uow = uow;                       
        }
        public PostViewModel(IUnitOfWork uow, string userID, int? pageNumber, int pageSize)
        {
            _uow = uow;
            PagedListPosts = _uow.Posts.GetByUserID(userID).ToPagedList(pageNumber ?? 1, pageSize);
        }

        public int PostID { get; set; }    
        [Required]
        [Display(Name = "Plate number")]
        public string PlateNumber { get; set; }
        [Required]
        public string Description { get; set; }               
        public DateTime DateUploaded { get; set; }
        [Required]
        public string UploadedBy { get; set; }
        public string EditedBy { get; set; }
        public DateTime? DateEdited { get; set; }
        public bool HasApproval { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? DateApproved { get; set; }
        public bool HasPlayedVideo { get; set; }
        public DateTime? DatePlayedVideo { get; set; }
        public string BranchName { get; set; }
        public string CurrentView { get; set; }

        public Post Post { get; set; }
        public IPagedList<Post> PagedListPosts { get; set; }


        public ICollection<PostAttachment> Attachments { get; set; }
        public ICollection<Post> ApprovedVideos { get; set; }
        public ICollection<Post> VideosPlayed { get; set; }
        
    }
}